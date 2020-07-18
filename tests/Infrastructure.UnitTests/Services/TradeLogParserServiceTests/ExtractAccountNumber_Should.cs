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
    public class ExtractAccountNumber_Should
    {
        public ExtractAccountNumber_Should()
        {
            _sut = new TradeLogTradeParserService(NUnitTestLogger.Create<TradeLogTradeParserService>(), A.Fake<IApplicationDbContext>());
        }

        private readonly TradeLogTradeParserService _sut;

        [Test]
        public void ReturnAccountNumber_WhenInputIsValid()
        {
            var input = "ACCOUNT_INFORMATION\n" +
                        "ACT_INF|U3111111|Mister Fakename|Individual|123 Lane St Kamloops BC V0Z 2C5 Canada\n";

            var result = _sut.ExtractAccountNumber(input);

            result.ShouldBe("U3111111");
        }

        [Test]
        [TestCase("")]
        [TestCase("ACT_INF||Name of person")]
        [TestCase("U3111111")]
        public void ReturnEmptyStringWhenInputIsInvalid(string input)
        {
            var result = _sut.ExtractAccountNumber(input);

            result.ShouldBe("");
        }
    }
}
