using AutoMapper;
using AutoMapper.QueryableExtensions;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using Firewatch.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Queries.GetTrades
{
    public class GetTradesQuery : PersonScopedAuthorizationRequiredRequest, IRequest<GetTradesVm>
    {
        public string AccountNumber { get; set; }
        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;
    }

    public class GetTradesHandler : IRequestHandler<GetTradesQuery, GetTradesVm>
    {
        private readonly ILogger<GetTradesHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTradesHandler(ILogger<GetTradesHandler> logger, IApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetTradesVm> Handle(GetTradesQuery request, CancellationToken cancellationToken)
        {
            var executions = await _context.TradeExecutions
                .Where(t => t.Account.OwnerId == request.OwnerId) // TODO: push to validation?
                .Where(t => t.Account.AccountNumber == request.AccountNumber)
                .Where(t => t.Date >= request.From)
                .Where(t => t.Date <= request.To)
                .ToListAsync();

            var groupedExecutions = executions
                .GroupBy(t => t.Symbol)
                .Select(g => new { Symbol = g.Key, Executions = g.AsEnumerable() });

            var trades = new List<Trade>();
            foreach (var group in groupedExecutions)
            {
                var trade = new Trade(group.Symbol);
                trades.Add(trade);
                foreach (var execution in group.Executions)
                {
                    if (trade.Executions.Count() == 0 || trade.State == TradeState.OPEN)
                    {
                        trade.AddExecutions(execution);
                    }
                    else
                    {
                        trades.Add(trade);
                        trade = new Trade(group.Symbol);
                        trade.AddExecutions(execution);
                    }
                }
            }

            


            return new GetTradesVm
            {
               Trades = _mapper.Map<List<TradeDto>>(trades)
            };
        }
    }
}
