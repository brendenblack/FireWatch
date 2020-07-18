using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Common.Models;
using Firewatch.Domain.Constants;
using Firewatch.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Commands.AddTrades
{
    public class CreateTradeCommand : PersonScopedAuthorizationRequiredRequest, IRequest<int>
    {
        //public bool StrictMode { get; set; } = true;
        public string AccountNumber { get; set; }

        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string Symbol { get; set; }

        public decimal Quantity { get; set; }

        public CostModel UnitPrice { get; set; } = new CostModel();

        public CostModel Commissions { get; set; } = new CostModel();

        public CostModel Fees { get; set; } = new CostModel();
    }

    public class AddTradeHandler : IRequestHandler<CreateTradeCommand, int>
    {
        private readonly ILogger<AddTradeHandler> _logger;
        private readonly IApplicationDbContext _context;

        public AddTradeHandler(ILogger<AddTradeHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
        {
            BrokerageAccount account = (BrokerageAccount) await _context.Accounts
                .Where(a => a.OwnerId == request.OwnerId)
                .Where(a => a.AccountNumber == request.AccountNumber)
                .FirstAsync();

            Price unitPrice, fees, commissions;

            try
            {
                unitPrice = new Price(request.UnitPrice.Amount, request.UnitPrice.Currency);
                fees = new Price(request.Fees.Amount, request.Fees.Currency);
                commissions = new Price(request.Commissions.Amount, request.Commissions.Currency);
            }
            catch (ArgumentException e)
            {
                // TODO this is a shit message
                _logger.LogWarning("Unable to create a trade because {}", e.Message);
                throw new ArgumentException();
            }

            if (!TradeConstants.SUPPORTED_TRADE_ACTIONS.Contains(request.Action.ToLower()))
            {
                _logger.LogWarning("Unsupported trade action: {}", request.Action);
            }

            var trade = new TradeExecution(account, request.Action, request.Date, request.Symbol, request.Quantity, unitPrice, commissions, fees);

            _context.TradeExecutions.Add(trade);
            await _context.SaveChangesAsync(cancellationToken);

            return trade.Id;
        }
    }
}
