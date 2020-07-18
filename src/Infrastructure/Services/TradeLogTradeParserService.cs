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
                var date = ConstructDateTime(record.Date, record.Time);

                // TODO
                var currency = "USD";
                var unitPrice = new Price(record.UnitPrice, currency);
                var commissions = new Price(record.Commissions, currency);
                var fees = new Price();

                var trade = new TradeExecution(account, record.Action, date, record.TickerSymbol, record.Quantity, unitPrice, commissions, fees);
                
                trades.Add(trade);
            }

            return trades;
        }

        public List<TradeLogRecord> ExtractRecords(string tradelog)
        {
            string relevantContents = ExtractTransactions(tradelog);

            using (var reader = new StringReader(relevantContents))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = "|";
                csv.Configuration.HasHeaderRecord = false;

                var records = csv.GetRecords<TradeLogRecord>().ToList();

                _logger.LogDebug("Parsed {} records from {} TradeLog lines",
                    records.Count,
                    relevantContents.Split('\n').Length);

                return records;
            }
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

        public string ExtractTransactions(string contents)
        {
            var lines = new List<string>();
            foreach (var line in contents.Split('\n'))
            {
                if (line.StartsWith("STK_TRD"))
                {
                    lines.Add(line);
                }
            }

            return string.Join('\n', lines);
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
        public List<string> Routes { get; set; } = new List<string>();

        [Index(5)]
        public string Action { get; set; }

        [Index(6)]
        public string ActionIdentifier { get; set; }

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
