using AutoMapper;
using Firewatch.Application.Common.Interfaces;
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

namespace Firewatch.Application.Investments.Commands.ParseAndImportTrades
{
    public class ParseAndImportTradesCommand : PersonScopedAuthorizationRequiredRequest, IRequest<ParseAndImportTradesResponse>
    {
        public string Format { get; set; }

        public string Contents { get; set; }
    }

    public class ParseAndImportTradesHandler : IRequestHandler<ParseAndImportTradesCommand, ParseAndImportTradesResponse>
    {
        private readonly ILogger<ParseAndImportTradesHandler> _logger;
        private readonly IEnumerable<ITradeParserService> _parsers;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ParseAndImportTradesHandler(
            ILogger<ParseAndImportTradesHandler> logger, 
            IEnumerable<ITradeParserService> parsers,
            IApplicationDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _parsers = parsers;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParseAndImportTradesResponse> Handle(ParseAndImportTradesCommand request, CancellationToken cancellationToken)
        {
            var owner = _context.People.First(p => p.Id == request.OwnerId);
            
            var parser = _parsers.First(p => p.Format.Equals(request.Format, StringComparison.InvariantCultureIgnoreCase));
            var parsedTrades = parser.ParseForOwner(owner, request.Contents)
                .GroupBy(t => t.Account.AccountNumber)
                .ToDictionary(k => k.Key, v => v.ToList());
            var addedTrades = new List<TradeExecution>();
            var response = new ParseAndImportTradesResponse();

            foreach (var groupedExecutions in parsedTrades)
            {
                var account = (BrokerageAccount) await _context.Accounts
                    .Where(a => a.OwnerId == request.OwnerId)
                    .Where(a => a.AccountNumber == groupedExecutions.Key)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    account = new BrokerageAccount(owner, groupedExecutions.Key);
                    _context.Accounts.Add(account);
                }

                foreach (var e in groupedExecutions.Value)
                {
                    var isDuplicate = _context.TradeExecutions
                        .Where(t => t.AccountId == account.Id)
                        .Where(t => t.Date == e.Date)
                        .Where(t => t.Symbol == e.Symbol)
                        .Any();

                    if (isDuplicate)
                    {
                        _logger.LogDebug("Ignoring duplicate trade [Account number: {}] [Symbol: {}] [Time: {}]",
                            account.AccountNumber,
                            e.Symbol,
                            e.Date);

                        response.Duplicates++;
                        continue;
                    }

                    // This is a hack to get around EF trying to save children entities that already exist (i.e. account)
                    var execution = new TradeExecution(account, e.Action, e.Date, e.Symbol, e.Quantity, e.UnitPrice, e.Commissions, e.Fees);

                    _context.TradeExecutions.Add(execution);
                    addedTrades.Add(execution);
                }               
            }

            await _context.SaveChangesAsync(cancellationToken);

            response.CreatedIds = addedTrades
                .Select(t => t.Id)
                .ToList();

            return response;
        }
    }
}
