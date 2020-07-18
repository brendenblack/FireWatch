using Firewatch.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Accounts.Queries.GetBalance
{
    public class GetBalanceQuery : IRequest<double>
    {
    }

    public class GetBalanceHandler : IRequestHandler<GetBalanceQuery, double>
    {
        private readonly IApplicationDbContext _context;

        public GetBalanceHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<double> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
