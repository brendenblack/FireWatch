using Firewatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application.UnitTests.Common.Services.TradeFactoryTests
{
    public class TradeTestCase
    {
        public TradeExecution[] Executions { get; set; }

        public int ExpectedTradeCount { get; set; }

        public int ExpectedDayCount { get; set; }

        public int ExpectedExecutions { get; set; }
    }
}
