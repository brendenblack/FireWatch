using FakeItEasy;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Infrastructure.Services;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Firewatch.Infrastructure.UnitTests.Services.TradeLogParserServiceTests
{
    public class ExtractRecords_Should
    {
        public ExtractRecords_Should()
        {
            _sut = new TradeLogTradeParserService(NUnitTestLogger.Create<TradeLogTradeParserService>(), A.Fake<IApplicationDbContext>());
        }

        [OneTimeSetUp]
        public void LoadTestFile()
        {
            ReadLocalTestFile("U3111111_with_options.tlg");
        }

        public void ReadLocalTestFile(string filename)
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, filename);

            _contents = File.ReadAllText(file);  
        }

        private readonly TradeLogTradeParserService _sut;
        private  string _contents;

        // These test case values come from manually parsing the test tlg file.
        // If the input file changes, these tests will likely fail.
        // STK_TRD|67428375|AAL|AMERICAN AIRLINES GROUP INC|ISLAND|SELLTOCLOSE|C|20200422|10:17:04|USD|-331.00|1.00|10.61|-3511.91|-1.772002|1.00
        // OPT_TRD|75293258|SPY   200722C00326000|SPY 22JUL20 326.0 C|CBOE2|BUYTOOPEN|O|20200722|09:35:20|USD|2.00|100.00|0.64|128.00|-0.6476|1.00
        [Test]
        [TestCase(67428375, "STK_TRD", "AAL", "SELLTOCLOSE", "20200422", "10:17:04", -331, 10.61, -1.772002)]
        [TestCase(75293258, "OPT_TRD", "SPY   200722C00326000", "BUYTOOPEN", "20200722", "09:35:20", 2, 0.64, -0.6476)]
         public void ParseRecords(
            int tradeId,
            string expectedTransactionType,
            string expectedSymbol,
            string expectedAction,
            string expectedDate,
            string expectedTime,
            decimal expectedQuantity,
            decimal expectedPrice,
            decimal expectedCommissions)
        {
            var records = _sut.ExtractRecords(_contents);

            var record = records.First(r => r.TransactionId == tradeId);
            record.TransactionType.ShouldBe(expectedTransactionType);
            record.TickerSymbol.ShouldBe(expectedSymbol);
            record.Action.ShouldBe(expectedAction);
            record.Date.ShouldBe(expectedDate);
            record.Time.ShouldBe(expectedTime);
            record.Quantity.ShouldBe(expectedQuantity);
            record.UnitPrice.ShouldBe(expectedPrice);
            record.Commissions.ShouldBe(expectedCommissions);
        }
    }


}
