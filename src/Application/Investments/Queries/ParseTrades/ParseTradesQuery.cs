using AutoMapper;
using Firewatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Investments.Queries.ParseTrades
{
    public class ParseTradesQuery : PersonScopedAuthorizationRequiredRequest, IRequest<ParseTradesResponse>
    {
        public string Format { get; set; }

        public string Content { get; set; }
    }

    public class ParseTradesHandler : IRequestHandler<ParseTradesQuery, ParseTradesResponse>
    {
        private readonly ILogger<ParseTradesHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IEnumerable<ITradeParserService> _parsers;
        private readonly IMapper _mapper;

        public ParseTradesHandler(
            ILogger<ParseTradesHandler> logger, 
            IApplicationDbContext context, 
            IEnumerable<ITradeParserService> parsers,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _parsers = parsers;
            _mapper = mapper;
        }

        public async Task<ParseTradesResponse> Handle(ParseTradesQuery request, CancellationToken cancellationToken)
        {
            var parser = _parsers.First(p => p.Format.Equals(request.Format, StringComparison.InvariantCultureIgnoreCase));
            
            var owner = _context.People.First(p => p.Id == request.OwnerId);
            
            var trades = parser.ParseForOwner(owner, request.Content);
            var dtos = _mapper.Map<List<ParsedTradeDto>>(trades);

            return new ParseTradesResponse { Trades = dtos };
        }
    }
}
