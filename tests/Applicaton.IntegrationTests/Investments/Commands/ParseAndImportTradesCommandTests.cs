using Firewatch.Application.Investments.Commands.ParseAndImportTrades;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Investments.Commands
{
    using static Testing;
    public class ParseAndImportTradesCommandTests : TestBase
    {
        [Test]
        public async Task ShouldReturnCreatedIds()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var contents = ReadLocalTestFile("U3111111_20200316_20200501.tlg");
            var command = new ParseAndImportTradesCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Contents = contents,
                Format = "TradeLog"
            };

            var result = await SendAsync(command);

            result.CreatedIds.Should().NotBeEmpty();
            result.CreatedIds.Count.Should().Be(748);
        }

        [Test]
        public async Task NotImportDuplicatesWhenTimestampAndSymbolMatch()
        {
            var userId = await RunAsDefaultUserAsync();
            //var owner = await FindAsync<Person>(userId);
            var contents = ReadLocalTestFile("U3111111_20200316_20200501.tlg");
            // STK_TRD|67428140|AAL|AMERICAN AIRLINES GROUP INC|ISLAND,BATS,DARK,ARCA|BUYTOOPEN|O|20200422|10:16:12|USD|331.00|1.00|10.596979|3507.60|-1.655|1.00
            var account = new BrokerageAccount(new Person { Id = userId }, "U3111111");
            var exec = new TradeExecution(account, new DateTime(2020, 04, 22, 10, 16, 12), "AAL", 331.0m, new Price());
            await AddAsync(exec);
            var command = new ParseAndImportTradesCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Contents = contents,
                Format = "TradeLog"
            };

            var result = await SendAsync(command);
            
            result.Duplicates.Should().Be(1);
            result.CreatedIds.Count.Should().Be(747);
        }

        [Test]
        public async Task ImportOptionsTrades()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var contents = ReadLocalTestFile("U3111111_with_options.tlg");
            var command = new ParseAndImportTradesCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Contents = contents,
                Format = "TradeLog"
            };

            var result = await SendAsync(command);

            result.CreatedIds.Count.Should().BeGreaterThan(14);
        }
    }
}
