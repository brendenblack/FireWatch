using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using Firewatch.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Firewatch.Infrastructure.Services
{
    public class TradeLogTradeParserService : ITradeParserService
    {
        private readonly ILogger<TradeLogTradeParserService> _logger;
        private readonly IApplicationDbContext _context;

        public TradeLogTradeParserService(ILogger<TradeLogTradeParserService> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public string Format => "TradeLog";

        
        

        public FinancialAssetTypes[] SupportedFinancialAssets => new[] { FinancialAssetTypes.STOCKS };

        public IEnumerable<TradeExecution> ParseForOwner(Person owner, string contents)
        {
            
            var accountNumber = ExtractAccountNumber(contents);
            var account = new BrokerageAccount(owner, accountNumber);
            account.Institution = "Interactive Brokers";
            var trades = new List<TradeExecution>();

            var records = ExtractRecords(contents);
            foreach (var record in records)
            {
                if (record.TransactionType != "STK_TRD" && record.TransactionType != "OPT_TRD")
                {
                    // TODO
                    continue;
                }

                var date = ConstructDateTime(record.Date, record.Time);

                var unitPrice = new Price(record.UnitPrice, record.Currency);
                var commissions = new Price(record.Commissions, record.Currency);
                var fees = new Price();

                var routes = record.Routes.Split(',');
                var codes = record.ActionIdentifiers.Split(';');

                TradeActions tradeAction;
                switch (record.Action.ToUpper())
                {
                    case "SELLTOOPEN":
                        tradeAction = TradeActions.SELL_TO_OPEN;
                        break;
                    case "SELLTOCLOSE":
                        tradeAction = TradeActions.SELL_TO_CLOSE;
                        break;
                    case "BUYTOCLOSE":
                        tradeAction = TradeActions.BUY_TO_CLOSE;
                        break;
                    case "BUYTOOPEN":
                    default:
                        tradeAction = TradeActions.BUY_TO_OPEN;
                        break;
                }

                TradeVehicle vehicle;
                var symbol = record.TickerSymbol;
                switch (record.TransactionType)
                {
                    case "STK_TRD":
                        vehicle = TradeVehicle.STOCK;
                        break;
                    case "OPT_TRD":
                        vehicle = TradeVehicle.OPTION;
                        // IB provides unparsable garbage in the symbol category, so we parse the company name instead.
                        // The problem is the unintelligible strike price, e.g. 'SPY   200722C00327000'; where should the 
                        // decimal go?! Of course you and I can see it's $327.00, but tell RegEx that. 
                        // It could be that the decimal is always 3 places in, but I don't want to build on that assumption 
                        // when this easy workaround exists.
                        var contract = OptionContract.For(record.CompanyName, IB_OPTION_NOMENCLATURE_PATTERN);
                        symbol = contract.ToString();
                        break;
                    case "CASH_TRD":
                        vehicle = TradeVehicle.CASH;
                        break;
                    default:
                        _logger.LogWarning("Unable to determine vehicle for this trade type ({}), skipping it.", record.TransactionType);
                        continue;
                }


                bool isPartial = codes.Contains("P");

                var trade = new TradeExecution(
                    account,
                    date,
                    symbol,
                    record.Quantity,
                    unitPrice,
                    commissions,
                    fees,
                    tradeAction,
                    isPartial,
                    routes,
                    vehicle,
                    TradeConstants.CREATION_METHOD_IMPORT);

                trades.Add(trade);
            }

            return trades;
        }

        public List<TradeLogRecord> ExtractRecords(string tradelog)
        {
            string stockTrades = ExtractStockTrades(tradelog);
            string optionTrades = ExtractOptionTrades(tradelog);

            var tradeRecords = new List<TradeLogRecord>();

            foreach (var trades in new[] { stockTrades, optionTrades })
            {
                using (var reader = new StringReader(trades))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = "|";
                    csv.Configuration.HasHeaderRecord = false;

                    var records = csv.GetRecords<TradeLogRecord>().ToList();

                    _logger.LogDebug("Parsed {} records from {} TradeLog lines",
                        records.Count,
                        stockTrades.Split('\n').Length);

                    tradeRecords.AddRange(records);
                }
            }

            return tradeRecords;           
        }

        public string ExtractAccountNumber(string tradelog)
        {
            var pattern = new Regex(@"ACT_INF\|(\w+)\|");

            var match = pattern.Match(tradelog);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                _logger.LogWarning("Unable to find an account number in provided TradeLog.");
                return "";
            }
        }

        public DateTime ConstructDateTime(string dateString, string timeString)
        {
            var input = $"{dateString} {timeString}";
            return DateTime.ParseExact(input, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public string ExtractStockTrades(string contents)
        {
            return ExtractTransactions(contents, "STK_TRD");
        }

        public string ExtractTransactions(string contents, string transactionCode)
        {
            var lines = new List<string>();
            foreach (var line in contents.Split('\n'))
            {
                if (line.StartsWith(transactionCode))
                {
                    lines.Add(line);
                }
            }

            return string.Join('\n', lines);
        }

        public string ExtractOptionTrades(string contents)
        {
            return ExtractTransactions(contents, "OPT_TRD");
        }


        public const string IB_OPTION_NOMENCLATURE_PATTERN = @"(?<symbol>\w+)\s+(?<day>\d{2})(?<month>[A-Z]+)(?<year>\d{2})\s(?<strike>\d+\.\d*)\s(?<type>\w)";
    }

    public class TradeLogRecord
    {
        [Index(0)]
        public string TransactionType { get; set; }

        [Index(1)]
        public int TransactionId { get; set; }

        [Index(2)]
        public string TickerSymbol { get; set; }

        [Index(3)]
        public string CompanyName { get; set; }

        [Index(4)]
        public string Routes { get; set; }

        [Index(5)]
        public string Action { get; set; }

        [Index(6)]
        public string ActionIdentifiers { get; set; }

        [Index(7)]
        public string Date { get; set; }

        [Index(8)]
        public string Time { get; set; }

        [Index(9)]
        public string Currency { get; set; }

        [Index(10)]
        public decimal Quantity { get; set; }

        [Index(11)]
        public decimal CurrencyExchangeRate { get; set; }

        [Index(12)]
        public decimal UnitPrice { get; set; }

        [Index(13)]
        public decimal BookValue { get; set; }

        [Index(14)]
        public decimal Commissions { get; set; }

        public override string ToString()
        {
            return $"{Action}: {TickerSymbol} x {Quantity} @ {UnitPrice}";
        }
    }
}

