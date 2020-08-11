using AutoMapper;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Common.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Queries.GetJournal
{
    public class GetJournalQuery : PersonScopedAuthorizationRequiredRequest, IRequest<JournalVm>
    {
        public int AccountId { get; set; }

        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;
    }

    public class GetJournalHandler : IRequestHandler<GetJournalQuery, JournalVm>
    {
        private readonly ILogger<GetJournalHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly TradeFactory _tradeFactory;
        private readonly IMapper _mapper;

        public GetJournalHandler(ILogger<GetJournalHandler> logger, IApplicationDbContext context, TradeFactory factory, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _tradeFactory = factory;
            _mapper = mapper;
        }

        public async Task<JournalVm> Handle(GetJournalQuery request, CancellationToken cancellationToken)
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
                .ToListAsync();

            var trades = _tradeFactory.ConstructTradesFromExecutions(executions);

            return new JournalVm();
        }
    }
}
