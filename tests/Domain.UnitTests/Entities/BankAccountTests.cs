using Firewatch.Domain.Entities;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.UnitTests.Entities
{
    public class BankAccountTests
    {
        [Theory]
        [TestCase("003111112222222", "003-11111-2222222")]
        public void Ctor_ShouldSetAccountNumber(string input, string expectedAccountNumber)
        {
            var owner = new Person { Id = Guid.NewGuid().ToString() };

            var account = new BankAccount(owner, input);

            account.AccountNumber.ShouldBe(expectedAccountNumber);
        }

        [Test]
        public void Ctor_ShouldSetOwner()
        {
            var owner = new Person { Id = Guid.NewGuid().ToString() };

            var account = new BankAccount(owner, "003111112222222");

            account.Owner.Id.ShouldBe(owner.Id);
        }
    }
}
