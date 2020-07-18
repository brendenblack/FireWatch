using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Commands.ImportTrades
{
    public class ImportTradesCommand : PersonScopedAuthorizationRequiredRequest, IRequest<ImportTradesResponse>
    {
        public List<ImportTradeModel> Trades { get; set; } = new List<ImportTradeModel>();
    }

    public class ImportTradesHandler : IRequestHandler<ImportTradesCommand, ImportTradesResponse>
    {
        private readonly ILogger<ImportTradesHandler> _logger;
        private readonly IApplicationDbContext _context;

        public ImportTradesHandler(ILogger<ImportTradesHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ImportTradesResponse> Handle(ImportTradesCommand request, CancellationToken cancellationToken)
        {
            var owner = _context.People.First(p => p.Id == request.OwnerId);
            var trades = new List<TradeExecution>();
            var response = new ImportTradesResponse();

            foreach (var model in request.Trades)
            {
                var isDuplicate = _context.TradeExecutions
                    //.Where(t => t.accountId == request.OwnerId)
                    .Where(t => t.Date == model.Date)
                    .Where(t => t.Symbol == model.Symbol)
                    .Any();

                if (isDuplicate)
                {
                    response.DuplicateCount++;
                    continue;
                }

                var unitPrice = new Price(model.UnitPrice.Amount, model.UnitPrice.Currency);
                var fees = new Price(model.Fees.Amount, model.Fees.Currency);
                var commissions = new Price(model.Commissions.Amount, model.Commissions.Currency);

                // TODO, whatever
                var trade = new TradeExecution(new BrokerageAccount(owner, ""), model.Action, model.Date, model.Symbol, model.Quantity, unitPrice, commissions, fees);
                trades.Add(trade);
            }

            return response;
        }

        
    }
}
