using Firewatch.Application.Accounts.Commands.CreateAccount;
using Firewatch.Application.Accounts.Commands.ModifyAccount;
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
    public class ModifyAccountCommandTests : TestBase
    {
        [Test]
        public async Task ShouldThrowWhenAccountNotFound()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ModifyAccountCommand
            {
                RequestorId = userId,
                OwnerId = userId,
                Id = 8,
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldModifyDisplayName()
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

            var command = new ModifyAccountCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Id = accountId,
                DisplayName = "Electric Boogaloo"
            };

            await SendAsync(command);

            var account = await FindAsync<Account>(accountId);
            account.Should().NotBeNull();
            account.DisplayName.Should().Be("Electric Boogaloo");
        }


        [Test]
        public async Task ShouldModifyBalanceOffset_WhenSupplied()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var accountId = await SendAsync(new CreateAccountCommand
            {
                PersonId = userId,
                DisplayName = "My Account 2",
                AccountNumber = "4511123456780123",
                AccountType = AccountConstants.CREDIT_CARD,
                BalanceOffset = 123.4m
            });

            var command = new ModifyAccountCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Id = accountId,
                Offset = 50.43m
            };

            await SendAsync(command);

            var account = await FindAsync<Account>(accountId);
            account.Should().NotBeNull();
            account.BalanceOffset.Should().Be(50.43m);
        }

        [Test]
        public async Task ShouldNotModifyBalanceOffset_WhenNotSupplied()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var accountId = await SendAsync(new CreateAccountCommand
            {
                PersonId = userId,
                DisplayName = "My Account 2",
                AccountNumber = "4511123456780123",
                AccountType = AccountConstants.CREDIT_CARD,
                BalanceOffset = 123.4m
            });

            var command = new ModifyAccountCommand
            {
                OwnerId = userId,
                RequestorId = userId,
                Id = accountId,
            };

            await SendAsync(command);

            var account = await FindAsync<Account>(accountId);
            account.Should().NotBeNull();
            account.BalanceOffset.Should().Be(123.4m);
        }
    }
}
