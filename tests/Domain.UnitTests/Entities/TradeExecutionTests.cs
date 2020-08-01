using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.UnitTests.Entities
{
    public class TradeExecutionTests
    {
        public class TotalValueTestCase
        {
            public TradeExecution Execution { get; set; }

            public decimal ExpectedTotalValue { get; set; }
        }

        public static IEnumerable<TotalValueTestCase> TotalValueTestCases => new List<TotalValueTestCase>
        {
            new TotalValueTestCase
            {
                Execution = new TradeExecution(new BrokerageAccount(), DateTime.Now, "AMD", 152, new Price(53.4m, "USD"), tradeAction: TradeActions.BUY_TO_OPEN),
                ExpectedTotalValue = -8116.8m
            },
            new TotalValueTestCase
            {
                Execution = new TradeExecution(new BrokerageAccount(), DateTime.Now, "AMD", -152, new Price(53.4m, "USD"), tradeAction: TradeActions.SELL_TO_CLOSE),
                ExpectedTotalValue = 8116.8m
            },
            new TotalValueTestCase
            {
                Execution = new TradeExecution(new BrokerageAccount(), DateTime.Now, "AMD", -3, new Price(0.57m, "USD"), tradeAction: TradeActions.SELL_TO_CLOSE, vehicle: TradeVehicle.OPTION),
                ExpectedTotalValue = 171.00m
            },
            new TotalValueTestCase
            {
                Execution = new TradeExecution(new BrokerageAccount(), DateTime.Now, "AMD", 4, new Price(1.07m, "USD"), tradeAction: TradeActions.BUY_TO_OPEN, vehicle: TradeVehicle.OPTION),
                ExpectedTotalValue = -428
            },
        };

        [Test]
        [TestCaseSource(nameof(TotalValueTestCases))]
        public void ShouldCalculateProperValue(TotalValueTestCase testCase)
        {
            testCase.Execution.TotalValue.ShouldBe(testCase.ExpectedTotalValue);
        }
    }
}
