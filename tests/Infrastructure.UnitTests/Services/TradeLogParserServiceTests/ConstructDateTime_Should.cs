using FakeItEasy;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Infrastructure.Services;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Infrastructure.UnitTests.Services.TradeLogParserServiceTests
{
    public class ConstructDateTime_Should
    {
        public ConstructDateTime_Should()
        {
            _sut = new TradeLogTradeParserService(NUnitTestLogger.Create<TradeLogTradeParserService>(), A.Fake<IApplicationDbContext>());
        }

        private readonly TradeLogTradeParserService _sut;

        public static IEnumerable<object[]> ValidInputs =>
            new List<object[]>
            {
                new object[] { "20200306", "10:26:35", new DateTime(2020, 03, 06, 10, 26, 35) }
            };

        [Test]
        [TestCaseSource(nameof(ValidInputs))]
        public void ReturnDate_WhenValidInput(string dateString, string timeString, DateTime expectedDateTime)
        {
            var result = _sut.ConstructDateTime(dateString, timeString);

            result.ShouldBe(expectedDateTime);
        }
    }
}
