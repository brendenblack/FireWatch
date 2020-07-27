using Firewatch.Application.Investments.Commands.ParseAndImportTrades;
using Firewatch.Application.Investments.Queries.GetTradeExecutions;
using Firewatch.Application.Investments.Queries.ParseTrades;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
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
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 22, 10, 16, 12), "AAL", 331.0m, new Price()));
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 22, 10, 17, 12), "AAL", 331.0m, new Price(), tradeAction: TradeActions.SELL_TO_CLOSE));
            db.SaveChanges();
            var query = new GetTradeExecutionsQuery
            {
                OwnerId = userId,
                RequestorId = userId,
            };

            var vm = await SendAsync(query);

            vm.Executions.Count.Should().Be(2);
        }

        [Test]
        public async Task ShouldReturnTradeExecutions_WhenFromAndToAreEqual()
        {
            var userId = await RunAsDefaultUserAsync();
            var db = CreateScopedContext();
            var account = new BrokerageAccount(new Person { Id = userId }, "U3111111");
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 22, 10, 16, 12), "AAL", 331.0m, new Price()));
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 22, 10, 17, 12), "AAL", 331.0m, new Price()));
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 23, 10, 17, 12), "AAL", 331.0m, new Price()));
            db.TradeExecutions.Add(new TradeExecution(account, new DateTime(2020, 04, 24, 10, 17, 12), "AAL", 331.0m, new Price()));
            db.SaveChanges();
            var query = new GetTradeExecutionsQuery
            {
                OwnerId = userId,
                RequestorId = userId,
                From = new DateTime(2020, 04, 22),
                To = new DateTime(2020, 04, 22)
            };

            var vm = await SendAsync(query);

            vm.Executions.Count.Should().Be(2);
        }

        [Test]
        public async Task ShouldReturnTradeExecutions_WhenFromAndToAreEqual2()
        {
            var userId = await RunAsDefaultUserAsync();
            //var db = CreateScopedContext();
            var account = new BrokerageAccount(new Person { Id = userId }, "U3111111");
            await AddAsync(account);
            //db.People.Add(owner);
            var contents = ReadLocalTestFile("U3111111_20200316_20200501.tlg");
            await SendAsync(new ParseAndImportTradesCommand
            {
                Contents = contents,
                OwnerId = userId,
                RequestorId = userId,
                Format = "TradeLog"
            });

            var query = new GetTradeExecutionsQuery
            {
                OwnerId = userId,
                RequestorId = userId,
                From = new DateTime(2020, 03, 16),
                To = new DateTime(2020, 03, 16)
            };

            var vm = await SendAsync(query);

            vm.Executions.Count.Should().Be(28);
        }
    }
}
