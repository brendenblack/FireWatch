using Firewatch.Application.Common.Services;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Application.UnitTests.Common.Services.TradeFactoryTests
{
    public class ConstructTradesFromExecutions_Should : TradeFactoryTestBase
    {
        public ConstructTradesFromExecutions_Should()
        {
            sut = new TradeFactory();
        }

        private readonly TradeFactory sut;

        public static IEnumerable<TradeTestCase> TradeTestCases => new List<TradeTestCase>
        {
            // combine a single open and single close together
            new TradeTestCase
            {
                Executions = new []
                {
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 30, 0), "AMD", 100, new Price()),
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 31, 0), "AMD", -100, new Price()),
                },
                ExpectedTradeCount = 1,
            },
            // two open/close pairs on the same symbol
            new TradeTestCase
            {
                Executions = new []
                {
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 30, 0), "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                    new TradeExecution(new BrokerageAccount(),  new DateTime(2020, 01, 01, 9, 31, 0), "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE),
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 32, 0), "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 33, 0), "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE),
                },
                ExpectedTradeCount = 2,
                
            },
            // multiple symbols
            new TradeTestCase
            {
                Executions = new []
                {
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 30, 0), "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                    new TradeExecution(new BrokerageAccount(),  new DateTime(2020, 01, 01, 9, 31, 0), "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE),
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 32, 0), "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                    new TradeExecution(new BrokerageAccount(), new DateTime(2020, 01, 01, 9, 33, 0), "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE),
                },
                ExpectedTradeCount = 2,

            },
        };
        
        //[Test]
        //[TestCaseSource(nameof(TradeTestCases))]
        //public void ShouldDo(TradeTestCase testCase)
        //{
        //    var trades = sut.ConstructTradesFromExecutions(testCase.Executions);

        //    trades.Count().Should().Be(testCase.ExpectedTradeCount);
        //}

        [Test]
        [TestCase("test1.tlg", 9)]
        [TestCase("test2.tlg", 6)]
        [TestCase("2020-07-27.tlg", 19)]
        public void ShouldCorrectlyAssembleTrades(string filename, int expectedTradeCount)
        {
            var executions = PopulateTradeExecutionsWithFile(filename);

            var trades = sut.ConstructTradesFromExecutions(executions);

            trades.Count().Should().Be(expectedTradeCount);
        }

        [Test]
        [TestCase("test1.tlg", 1)]
        public void ShouldCorrectlyIdentifySwingTrades(string filename, int expectedSwingTradeCount)
        {
            var executions = PopulateTradeExecutionsWithFile(filename);

            var trades = sut.ConstructTradesFromExecutions(executions);

            
            trades.Where(t => !t.IsIntraDay).Count().Should().Be(expectedSwingTradeCount);
        }

        [Test]
        [TestCase("test1.tlg", 1)]
        public void ShouldCorrectlyIdentifyShortTrades(string filename, int expectedShortTradeCount)
        {
            var executions = PopulateTradeExecutionsWithFile(filename);

            var trades = sut.ConstructTradesFromExecutions(executions);
            
            var trade = trades.Where(t => t.Symbol == "WKHS").First();

            trades.Where(t => t.Side == TradeSides.SHORT).Count().Should().Be(expectedShortTradeCount);
        }


    }
}
