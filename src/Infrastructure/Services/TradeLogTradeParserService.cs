using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
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
                if (record.TransactionType != "STK_TRD")
                {
                    continue;
                }

                var date = ConstructDateTime(record.Date, record.Time);

                var unitPrice = new Price(record.UnitPrice, record.Currency);
                var commissions = new Price(record.Commissions, record.Currency);
                var fees = new Price();

                var routes = record.Routes.Split(',');
                var codes = record.ActionIdentifiers.Split(';');
                TradeStatus tradeStatus;
                if (codes.Contains("O"))
                {
                    tradeStatus = TradeStatus.OPEN;
                } 
                else if (codes.Contains("C"))
                {
                    tradeStatus = TradeStatus.CLOSE;
                }
                else
                {
                    _logger.LogWarning("Unable to determine if trade was an open or close", codes);
                    continue;
                }

                TradeVehicle vehicle;
                switch (record.TransactionType)
                {
                    case "STK_TRD":
                        vehicle = TradeVehicle.STOCK;
                        break;
                    case "OPT_TRD":
                        vehicle = TradeVehicle.OPTION;
                        break;
                    default:
                        continue;
                }


                bool isPartial = codes.Contains("P");

                var trade = new TradeExecution(
                    account, 
                    record.Action, 
                    tradeStatus, 
                    date, 
                    record.TickerSymbol, 
                    record.Quantity, 
                    unitPrice, 
                    commissions,
                    fees, 
                    isPartial, 
                    routes);

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

