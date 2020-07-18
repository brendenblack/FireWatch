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
                new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(), new Price(), new Price()),
                new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(), new Price(), new Price()),
                new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 100m, new Price(), new Price(), new Price()));

            trade.Volume.ShouldBe(200m);
        }

        public class TradeTestCase
        {
            public TradeExecution[] Executions { get; set; }
            public decimal ExpectedVolume { get; set; }

            public int ExpectedExecutionCount { get; set; }

            public decimal ExpectedPositionSize { get; set; }

            public TradePositionStatus ExpectedStatus { get; set; }

            public decimal ExpectedNetProfitAndLoss { get; set; }

            public decimal ExpectedGrossProfitAndLoss { get; set; }
        }

        public static IEnumerable<TradeTestCase> TradeTestCases
        {
            get
            {
                var account = new BrokerageAccount(new Person(), "");
                return new List<TradeTestCase>
                {
                    // Straight-forward
                    new TradeTestCase
                    {
                        Executions = new []
                        {
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", -100m, new Price(54m, "USD"), new Price(3m, "USD"), new Price())
                        },
                        ExpectedVolume = 200m,
                        ExpectedPositionSize = 0m,
                        ExpectedStatus = TradePositionStatus.CLOSED,
                        ExpectedGrossProfitAndLoss = 400m,
                        ExpectedNetProfitAndLoss = 391m,
                        ExpectedExecutionCount = 3,
                        
                    },
                    // Unrelated executions (should be ignored)
                    new TradeTestCase
                    {
                        Executions = new []
                        {
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", 50m, new Price(50m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AAPL", 10m, new Price(-3000m, "USD"), new Price(3m, "USD"), new Price()),
                            new TradeExecution(account, TradeConstants.BUY_TO_OPEN, DateTime.Now, "AMD", -100m, new Price(54m, "USD"), new Price(3m, "USD"), new Price())
                        },
                        ExpectedVolume = 200m,
                        ExpectedPositionSize = 0m,
                        ExpectedStatus = TradePositionStatus.CLOSED,
                        ExpectedGrossProfitAndLoss = 400m,
                        ExpectedNetProfitAndLoss = 391m,
                        ExpectedExecutionCount = 3,
                    }
                };
            }
        }


        [TestCaseSource(nameof(TradeTestCases))]
        public void ShouldCalculateExpectedValues(TradeTestCase testCase)
        {
            var trade = new Trade("AMD");
            trade.AddExecutions(testCase.Executions.ToArray());

            trade.ExecutionCount.ShouldBe(testCase.ExpectedExecutionCount);
            trade.PositionSize.ShouldBe(testCase.ExpectedPositionSize);
            trade.Volume.ShouldBe(testCase.ExpectedVolume);
            trade.Status.ShouldBe(testCase.ExpectedStatus);
            trade.NetProfitAndLoss.ShouldBe(testCase.ExpectedNetProfitAndLoss);
            trade.GrossProfitAndLoss.ShouldBe(testCase.ExpectedGrossProfitAndLoss);
        }


    }
}
