using Firewatch.Domain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firewatch.Application.Investments.Commands.AddTrades
{
    public class CreateTradeCommandValidator : AbstractValidator<CreateTradeCommand>
    {
        public CreateTradeCommandValidator()
        {
            RuleFor(c => c.Action)
                .NotEmpty()
                .WithMessage("A trade action must be provided.");

            RuleFor(c => c.Action)
                .Must(a => TradeConstants.SUPPORTED_TRADE_ACTIONS.Contains(a.ToLower()))
                .WithSeverity(Severity.Warning);
        }
    }
}
