using Firewatch.Domain.Entities;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.UnitTests.Entities
{
    public class CreditCardAccountTests
    {
        [Theory]
        [TestCase("4500123457890123", "4500 **** **** 0123")]
        [TestCase("4500 1234 5789 0123", "4500 **** **** 0123")]
        [TestCase("4500123457890123    ", "4500 **** **** 0123")]
        [TestCase("    4500123457890123", "4500 **** **** 0123")]
        [TestCase("4 5 0 0 1 2 3 4 5 7 8 9 0 1 2 3", "4500 **** **** 0123")]
        [TestCase("4500********0123", "4500 **** **** 0123")]
        [TestCase("4500 **** **** 0123", "4500 **** **** 0123")]
        public void MaskAccountNumber_ShouldMaskMiddleDigits(string input, string expected)
        {
            CreditCardAccount.MaskAccountNumber(input).ShouldBe(expected);
        }

        [Theory]
        [TestCase("4500123457890123", "4500 **** **** 0123")]
        [TestCase("4500 1234 5789 0123", "4500 **** **** 0123")]
        [TestCase("4500123457890123    ", "4500 **** **** 0123")]
        [TestCase("    4500123457890123", "4500 **** **** 0123")]
        [TestCase("4 5 0 0 1 2 3 4 5 7 8 9 0 1 2 3", "4500 **** **** 0123")]
        [TestCase("4500********0123", "4500 **** **** 0123")]
        [TestCase("4500 **** **** 0123", "4500 **** **** 0123")]
        public void Ctor_ShouldSetValues(string input, string expectedAccountNumber)
        {
            var owner = new Person { Id = Guid.NewGuid().ToString() };

            var account = new CreditCardAccount(owner, input);

            account.AccountNumber.ShouldBe(expectedAccountNumber);
            account.DisplayName.ShouldBe(expectedAccountNumber);
            account.OwnerId.ShouldBe(owner.Id);
        }
    }
}
