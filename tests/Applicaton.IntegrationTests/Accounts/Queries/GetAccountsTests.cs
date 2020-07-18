using Firewatch.Application.Accounts.Commands.CreateAccount;
using Firewatch.Application.Accounts.Queries.GetAccounts;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using FluentAssertions;
using IdentityModel.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Accounts.Queries
{
    using static Testing;
    public class GetAccountsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAccounts()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            await SendAsync(new CreateAccountCommand
            {
                DisplayName = "my account",
                AccountNumber = "010110019999999",
                PersonId = userId,
                AccountType = AccountConstants.CHEQUING
            });
            var query = new GetAccountsQuery
            {
                OwnerId = userId,
                RequestorId = userId,
            };

            var result = await SendAsync(query);

            result.Accounts.Should().NotBeEmpty();
            var account = result.Accounts.First();
            account.DisplayName.Should().Be("my account");
            account.Type.Should().Be(AccountConstants.CHEQUING);
        }
    }
}
