using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Accounts.Commands.CreateAccount
{
    public partial class CreateAccountCommand : IRequest<int>
    {
        public string PersonId { get; set; }

        public string AccountNumber { get; set; }

        public string? DisplayName { get; set; }

        public decimal? BalanceOffset { get; set; }

        /// <summary>
        /// See <see cref="AccountConstants.SUPPORTED_ACCOUNT_TYPES"/> for supported values.
        /// </summary>
        public string AccountType { get; set; }
    }

    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly ILogger<CreateAccountHandler> _logger;
        private readonly IApplicationDbContext _context;

        public CreateAccountHandler(ILogger<CreateAccountHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var owner = _context.People.First(p => p.Id == request.PersonId);

            Account account;
            if (!AccountConstants.SUPPORTED_ACCOUNT_TYPES.Contains(request.AccountType))
            {
                _logger.LogWarning("Requested account type '{}' is not a known supported type.", request.AccountType);
            }

            switch (request.AccountType)
            {
                case AccountConstants.CREDIT_CARD:
                    account = new CreditCardAccount(owner, request.AccountNumber);
                    break;
                case AccountConstants.CHEQUING:
                case AccountConstants.SAVINGS:
                case AccountConstants.CASH:
                default:
                    account = new BankAccount(owner, request.AccountNumber);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(request.DisplayName))
            {
                account.DisplayName = request.DisplayName;
            }

            account.BalanceOffset = request.BalanceOffset ?? 0.0m;


            _context.Accounts.Add(account);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account {} was created with id {} for user {}",
                account.DisplayName,
                account.Id,
                account.OwnerId);

            return account.Id;
        }
    }
}
