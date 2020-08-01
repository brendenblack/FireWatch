using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Common.Services;
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

namespace Firewatch.Application.Investments.Queries.GetStats
{
    public class GetStatsQuery : PersonScopedAuthorizationRequiredRequest, IRequest<StatsVm>
    {
        public string AccountNumber { get; set; }

        public DateTime? CutoffDate { get; set; }
    }

    public class GetStatsHandler : IRequestHandler<GetStatsQuery, StatsVm>
    {
        private readonly ILogger<GetStatsHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly TradeFactory _tradeFactory;

        public GetStatsHandler(ILogger<GetStatsHandler> logger, IApplicationDbContext context, TradeFactory tradeFactory)
        {
            _logger = logger;
            _context = context;
            _tradeFactory = tradeFactory;
        }

        public async Task<StatsVm> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TradeExecutions
                .Where(e => e.Account.AccountNumber == request.AccountNumber);

            if (request.CutoffDate.HasValue)
            {
                query = query.Where(e => e.Date >= request.CutoffDate.Value);
            }

            var executions = await query.ToListAsync();

            var trades = _tradeFactory.ConstructTradesFromExecutions(executions);
                //.GroupBy(t => t.Close.DayOfWeek);


            var winners = trades
                .Where(t => t.NetProfitAndLoss > 0)
                .OrderByDescending(t => t.NetProfitAndLoss);

            var biggestWinner = trades.FirstOrDefault();

            var averageWinningAmount = winners
                .Select(t => t.NetProfitAndLoss)
                .Average();



            var losers = trades
                .Where(t => t.NetProfitAndLoss < 0)
                .OrderBy(t => t.NetProfitAndLoss);

            var biggestLoser = losers.FirstOrDefault();

            var averageLosingAmount = losers
                .Select(t => t.NetProfitAndLoss)
                .Average();

            var losingTrades = losers.Count();


            // daily view
            foreach (var dow in Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>())
            {
                var dowExecutions = executions.Where(e => e.Date.DayOfWeek == dow);
            }

            throw new NotImplementedException();
        }
    }
}
