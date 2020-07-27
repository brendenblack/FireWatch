using Firewatch.Domain.Enums;
using Firewatch.Domain.ValueObjects;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.UnitTests.ValueObjects
{
    public class OptionContractTests
    {
        [Test]
        [TestCase("SPY 22JUL20 327.0 C", @"(?<symbol>\w+)\s+(?<day>\d{2})(?<month>[A-Z]+)(?<year>\d{2})\s(?<strike>\d+\.\d*)\s(?<type>\w)", "SPY", 2020, 7, 22, 327.00, OptionTypes.CALL)]
        public void ShouldCreateExpectedContract_WhenPatternIsSuitable(string input, 
            string pattern,
            string expectedSymbol, 
            int expectedYear, 
            int expectedMonth, 
            int expectedDay,
            decimal expectedStrike,
            OptionTypes expectedType)
        {
            var contract = OptionContract.For(input, pattern);

            contract.Symbol.ShouldBe(expectedSymbol);
            contract.StrikePrice.Amount.ShouldBe(expectedStrike);
            contract.ExpirationDate.Date.ShouldBe(new DateTime(expectedYear, expectedMonth, expectedDay).Date);
            contract.OptionType.ShouldBe(expectedType);
        }

        [Test]
        public void ShouldProduceExpectedToString()
        {

        }

        [Test]
        public void ShouldToStringParsableByDomainFormat()
        {
            var contract1 = OptionContract.For("AMD", DateTime.Now, 123, OptionTypes.PUT);
            var input = contract1.ToString();

            var contract2 = OptionContract.For(input, OptionContract.DOMAIN_PATTERN);

            contract2.Symbol.ShouldBe(contract1.Symbol);
            contract2.StrikePrice.Amount.ShouldBe(contract1.StrikePrice.Amount);
            contract2.ExpirationDate.Date.ShouldBe(contract1.ExpirationDate.Date);
            contract2.OptionType.ShouldBe(contract1.OptionType);
        }
    }
}
