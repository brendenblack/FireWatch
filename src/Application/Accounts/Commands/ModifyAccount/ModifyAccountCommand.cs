using Firewatch.Application.Common.Exceptions;
using Firewatch.Application.Common.Interfaces;
using Firewatch.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Accounts.Commands.ModifyAccount
{
    public class ModifyAccountCommand : PersonScopedAuthorizationRequiredRequest, IRequest
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public decimal? Offset { get; set; }
    }

    public class ModifyAccountHandler : IRequestHandler<ModifyAccountCommand>
    {
        private readonly ILogger<ModifyAccountHandler> _logger;
        private readonly IApplicationDbContext _context;

        public ModifyAccountHandler(ILogger<ModifyAccountHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Unit> Handle(ModifyAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FindAsync(request.Id);

            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.Id);
            }

            account.DisplayName = request.DisplayName;

            if (request.Offset.HasValue)
            {
                account.BalanceOffset = request.Offset.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
