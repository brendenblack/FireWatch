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

namespace Firewatch.Application.Accounts.Queries.GetAccounts
{
    public class GetAccountsQuery : PersonScopedAuthorizationRequiredRequest, IRequest<AccountsVm>
    {
        public bool IncludeTransactions { get; set; } = false;
    }

    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, AccountsVm>
    {
        private readonly ILogger<GetAccountsHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAccountsHandler(ILogger<GetAccountsHandler> logger, IApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountsVm> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _context.Accounts
                .Where(a => a.OwnerId == request.OwnerId)
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new AccountsVm
            {
                Accounts = accounts
            };
            
        }
    }
}
