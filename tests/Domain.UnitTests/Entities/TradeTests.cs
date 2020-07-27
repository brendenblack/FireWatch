using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.UnitTests.Entities
{
    public class TradeTests
    {

        [Test]
        public void Open_ShouldReturnMin_WhenExecutionsEmpty()
        {
            var trade = new Trade("AMD");

            trade.Executions.Count().ShouldBe(0);
            trade.Open.ShouldBe(DateTime.MinValue);
        }

        [Test]
        public void Close_ShouldReturnMin_WhenExecutionsEmpty()
        {
            var trade = new Trade("AMD");

            trade.Executions.Count().ShouldBe(0);
            trade.Close.ShouldBe(DateTime.MinValue);
        }

        [Test]
        public void ShouldCalculateVolume()
        {
            var owner = new Person { Id = Guid.NewGuid().ToString() };
            var account = new BrokerageAccount(owner, "");
            var trade = new Trade("AMD");
            trade.AddExecutions(
                new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price()),
                new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price()),
                new TradeExecution(account, DateTime.Now, "AMD", 100m, new Price()));

            trade.Volume.ShouldBe(200m);
        }

        public class IndividualTradeTestCase
        {
            public TradeExecution[] Executions { get; set; }
            public decimal ExpectedVolume { get; set; }

            public int ExpectedExecutionCount { get; set; }

            public decimal ExpectedPositionSize { get; set; }

            public TradeState ExpectedStatus { get; set; }

            public decimal ExpectedNetProfitAndLoss { get; set; }

            public decimal ExpectedGrossProfitAndLoss { get; set; }
        }

        public static IEnumerable<IndividualTradeTestCase> IndividualTradeTestCases
        {
            get
            {
                var account = new BrokerageAccount(new Person(), "");
                return new List<IndividualTradeTestCase>
                {
                    // Straight-forward
                    new IndividualTradeTestCase
                    {
                        Executions = new []
                        {
                            new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD")),
                            new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD")),
                            new TradeExecution(account, DateTime.Now, "AMD", -100m, new Price(54m, "USD"), new Price(3m, "USD"))
                        },
                        ExpectedVolume = 200m,
                        ExpectedPositionSize = 0m,
                        ExpectedStatus = TradeState.CLOSED,
                        ExpectedGrossProfitAndLoss = 400m,
                        ExpectedNetProfitAndLoss = 391m,
                        ExpectedExecutionCount = 3,
                        
                    },
                    // Unrelated executions (should be ignored)
                    new IndividualTradeTestCase
                    {
                        Executions = new []
                        {
                            new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, DateTime.Now, "AAPL", 10m, new Price(-3000m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, DateTime.Now, "AMD", -100m, new Price(54m, "USD"), new Price(3m, "USD"), new Price())
                        },
                        ExpectedVolume = 200m,
                        ExpectedPositionSize = 0m,
                        ExpectedStatus = TradeState.CLOSED,
                        ExpectedGrossProfitAndLoss = 400m,
                        ExpectedNetProfitAndLoss = 391m,
                        ExpectedExecutionCount = 3,
                    }
                };
            }
        }

        [Test]
        [TestCaseSource(nameof(IndividualTradeTestCases))]
        public void ShouldCalculateExpectedValues(IndividualTradeTestCase testCase)
        {
            var trade = new Trade("AMD");
            trade.AddExecutions(testCase.Executions.ToArray());

            trade.ExecutionCount.ShouldBe(testCase.ExpectedExecutionCount);
            trade.Position.ShouldBe(testCase.ExpectedPositionSize);
            trade.Volume.ShouldBe(testCase.ExpectedVolume);
            trade.State.ShouldBe(testCase.ExpectedStatus);
            trade.NetProfitAndLoss.ShouldBe(testCase.ExpectedNetProfitAndLoss);
            trade.GrossProfitAndLoss.ShouldBe(testCase.ExpectedGrossProfitAndLoss);
        }


        [Test]
        public void ShouldProperlyIdentifyIntraday()
        {
            var account = new BrokerageAccount(new Person { Id = Guid.NewGuid().ToString() }, "1111111");
            var sut = new Trade("AMD");
            sut.AddExecutions(
                new TradeExecution(account, DateTime.Now, "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                new TradeExecution(account, DateTime.Now, "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE)
            );

            sut.IsClosed.ShouldBeTrue();
            sut.IsIntraDay.ShouldBeTrue();
            sut.IsSwing.ShouldBeFalse();
        }

        [Test]
        public void ShouldProperlyIdentifySwing()
        {
            var account = new BrokerageAccount(new Person { Id = Guid.NewGuid().ToString() }, "1111111");
            var sut = new Trade("AMD");
            sut.AddExecutions(
                new TradeExecution(account, new DateTime(2020, 1, 1), "AMD", 100, new Price(), tradeAction: TradeActions.BUY_TO_OPEN),
                new TradeExecution(account, new DateTime(2020, 1, 3), "AMD", -100, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE)
            );

            sut.IsClosed.ShouldBeTrue();
            sut.IsIntraDay.ShouldBeFalse();
            sut.IsSwing.ShouldBeTrue();
        }

    }
}
