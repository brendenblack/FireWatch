using Firewatch.Application.Investments.Queries.GetTradeExecutions;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Investments.Queries
{
    using static Testing;

    public class GetTradeExecutionsQueryTests : TestBase
    {
        [Test]
        public async Task ShouldReturnTradeExecutions()
        {
            var userId = await RunAsDefaultUserAsync();
            var db = CreateScopedContext();
            var account = new BrokerageAccount(new Person { Id = userId }, "U3111111");
            //db.People.Add(owner);
            db.TradeExecutions.Add(new TradeExecution(account, TradeConstants.BUY_TO_OPEN, new DateTime(2020, 04, 22, 10, 16, 12), "AAL", 331.0m, new Price(), new Price(), new Price()));
            db.TradeExecutions.Add(new TradeExecution(account, TradeConstants.SELL_TO_CLOSE, new DateTime(2020, 04, 22, 10, 17, 12), "AAL", 331.0m, new Price(), new Price(), new Price()));
            db.SaveChanges();
            var query = new GetTradeExecutionsQuery
            {
                OwnerId = userId,
                RequestorId = userId,
            };

            var vm = await SendAsync(query);

            vm.Executions.Count.Should().Be(2);
        }
    }
}
