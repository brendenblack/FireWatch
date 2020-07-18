using Firewatch.Application.Accounts.Commands.CreateAccount;
using Firewatch.Application.Common.Exceptions;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests.Accounts.Commands
{
    using static Testing;
    public class CreateAccountCommandTests : TestBase
    {
        [Test]
        public void ShouldThrowWhenInvalidAccount()
        {
            var command = new CreateAccountCommand
            {
                AccountType = "unsupportedtype",
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateAccount()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new Person { Id = userId });
            var command = new CreateAccountCommand
            {
                PersonId = userId,
                DisplayName = "My Account 2",
                AccountNumber = "4511123456780123",
                AccountType = AccountConstants.CREDIT_CARD,
            };

            var accountId = await SendAsync(command);

            var account = await FindAsync<Account>(accountId);
            account.Should().NotBeNull();
            account.GetType().Should().Be(typeof(CreditCardAccount));
            account.DisplayName.Should().Be("My Account 2");
            account.AccountNumber.Should().Be(CreditCardAccount.MaskAccountNumber("4511 **** **** 0123"));
        }
    }
}
