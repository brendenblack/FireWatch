using Firewatch.Application.Investments.Queries.ParseTrades;
using Firewatch.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Investments.Queries
{
    using static Testing;
    public class ParseTradesQueryTests : TestBase
    {
        [Test]
        public async Task ShouldReturnTrades_WhenFormatSupported()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var contents = ReadLocalTestFile("U3111111_20200316_20200501.tlg");
            var query = new ParseTradesQuery
            {
                Content = contents,
                OwnerId = userId,
                RequestorId = userId,
                Format = "TradeLog"
            };

            var response = await SendAsync(query);

            response.Trades.Should().NotBeEmpty();
        }

        [Test]
        [TestCase(2020, 03, 16, 09, 55, 33, "AMD", "SELLTOCLOSE", -50, 40.59, -1.050802)]
        public async Task ShouldReturnExpectedTrades_WhenFormatSupported(
            int year,
            int month, 
            int day,
            int hour,
            int minute,
            int second,
            string expectedSymbol,
            string expectedAction,
            decimal expectedQuantity,
            decimal expectedPrice,
            decimal expectedCommissions)
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var contents = ReadLocalTestFile("U3111111_20200316_20200501.tlg");
            var timestamp = new DateTime(year, month, day, hour, minute, second);
            var query = new ParseTradesQuery
            {
                Content = contents,
                OwnerId = userId,
                RequestorId = userId,
                Format = "TradeLog"
            };

            var response = await SendAsync(query);
            var trade = response.Trades.First(t => t.Date == timestamp);
            trade.Symbol.Should().Be(expectedSymbol);
            trade.Quantity.Should().Be(expectedQuantity);
            trade.UnitPrice.Amount.Should().Be(expectedPrice);
        }
    }
}
