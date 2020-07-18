using Firewatch.Application.Accounts.Commands.CreateAccount;
using Firewatch.Application.Accounts.Commands.DeleteAccount;
using Firewatch.Application.Common.Exceptions;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Accounts.Commands
{
    using static Testing;
    public class DeleteAccountCommandTests : TestBase
    {
        [Test]
        public async Task ShouldThrowWhenAccountNotFound()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new DeleteAccountCommand
            {
                RequestorId = userId,
                OwnerId = userId,
                Id = 8,
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteAccount()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var accountId = await SendAsync(new CreateAccountCommand
            {
                PersonId = userId,
                DisplayName = "My Account 2",
                AccountNumber = "4511123456780123",
                AccountType = AccountConstants.CREDIT_CARD,
            });
            var command = new DeleteAccountCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Id = accountId,
            };

            await SendAsync(command);

            var account = await FindAsync<Account>(accountId);
            account.Should().BeNull();
        }
    }
}
