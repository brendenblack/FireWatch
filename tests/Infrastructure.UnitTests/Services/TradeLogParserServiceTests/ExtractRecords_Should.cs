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
        public void ReadLocalTestFile()
        {
            var filename = "U3111111_20200316_20200501.tlg";
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, filename);

            _contents = File.ReadAllText(file);  
        }

        private readonly TradeLogTradeParserService _sut;
        private  string _contents;

        [Test]
        public void ReturnValidRecords()
        {
            var records = _sut.ExtractRecords(_contents);

            records.Count().ShouldBe(748);
        }

        // These test case values come from manually parsing the test tlg file.
        // If the input file changes, these tests will fail.
        [Test]
        [TestCase(64317112, "AMD", "SELLTOCLOSE", "20200316", "09:55:33", -50, 40.59, -1.050802)]
        // STK_TRD|64317112|AMD|ADVANCED MICRO DEVICES|DRCTEDGE|SELLTOCLOSE|C|20200316|09:55:33|USD|-50.00|1.00|40.59|-2029.50|-1.050802|1.00
        public void ParseRecords(
            int tradeId,
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
