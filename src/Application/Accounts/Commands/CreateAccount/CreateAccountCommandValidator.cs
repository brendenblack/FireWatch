using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateAccountCommandValidator(IApplicationDbContext context)
        {
            RuleFor(c => c.AccountType)
                .Must(a => AccountConstants.SUPPORTED_ACCOUNT_TYPES.Contains(a));

            RuleFor(c => c.AccountNumber)
                .Must((command, number) => !_context.Accounts
                    .Where(a => a.AccountNumber == command.AccountNumber)
                    .Where(a => a.OwnerId == command.PersonId)
                    .Any())
                .WithMessage("An account with that number already exists.");

            RuleFor(c => c.PersonId)
                .Must(PersonMustExist)
                .WithMessage(c => $"No user with id {c.PersonId} exists.");
            _context = context;
        }

        public bool PersonMustExist(string personId)
        {
            return _context.People.Any(p => p.Id == personId);
        }
    }
}
