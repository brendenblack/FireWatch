using AutoMapper;
using AutoMapper.QueryableExtensions;
using Firewatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Queries.GetTradeExecutions
{
    public class GetTradeExecutionsQuery : PersonScopedAuthorizationRequiredRequest, IRequest<TradeExecutionsVm>
    {
        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;

#nullable enable
        public string? AccountNumber { get; set; }
#nullable disable
    }

    public class GetTradeExecutionsHandler : IRequestHandler<GetTradeExecutionsQuery, TradeExecutionsVm>
    {
        private readonly ILogger<GetTradeExecutionsHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTradeExecutionsHandler(ILogger<GetTradeExecutionsHandler> logger, IApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TradeExecutionsVm> Handle(GetTradeExecutionsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TradeExecutions
                .Where(t => t.Account.OwnerId == request.OwnerId)
                .Where(t => t.Date >= request.From)
                .Where(t => t.Date <= request.To);

            if (!string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                query.Where(t => t.Account.AccountNumber.Equals(request.AccountNumber));
            }

            var executions = await query
                .ProjectTo<TradeExecutionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new TradeExecutionsVm
            {
                Executions = executions
            };
        }
    }
}
