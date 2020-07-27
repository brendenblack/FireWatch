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
    public class ConstructTradesFromExecutions_Should
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
        
        [Test]
        [TestCaseSource(nameof(TradeTestCases))]
        public void ShouldDo(TradeTestCase testCase)
        {
            var trades = sut.ConstructTradesFromExecutions(testCase.Executions);

            trades.Count().Should().Be(testCase.ExpectedTradeCount);
        }
    }
}
