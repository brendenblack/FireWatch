using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace Firewatch.Application.Investments.Queries.GetTrades
{
    public class GetTradesQuery : PersonScopedAuthorizationRequiredRequest, IRequest<GetTradesVm>
    {
        public int AccountId { get; set; }
        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;
    }

    public class GetTradesHandler : IRequestHandler<GetTradesQuery, GetTradesVm>
    {
        private readonly ILogger<GetTradesHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly TradeFactory _tradeFactory;

        public GetTradesHandler(ILogger<GetTradesHandler> logger, IApplicationDbContext context, IMapper mapper, TradeFactory tradeFactory)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _tradeFactory = tradeFactory;
        }

        public async Task<GetTradesVm> Handle(GetTradesQuery request, CancellationToken cancellationToken)
        {
            if (request.From == request.To)
            {
                request.From = new DateTime(request.From.Year, request.From.Month, request.From.Day, 0, 0, 0);
                request.To = new DateTime(request.To.Year, request.To.Month, request.To.Day, 23, 59, 59);
            }

            var executions = await _context.TradeExecutions
                .Where(t => t.Account.OwnerId == request.OwnerId) // TODO: push to validation?
                .Where(t => t.AccountId == request.AccountId)
                .Where(t => t.Date >= request.From)
                .Where(t => t.Date <= request.To)
                .ToListAsync(cancellationToken);

            var trades = _tradeFactory.ConstructTradesFromExecutions(executions);

            return new GetTradesVm
            {
               Trades = _mapper.Map<List<TradeDto>>(trades)
            };
        }
    }
}
