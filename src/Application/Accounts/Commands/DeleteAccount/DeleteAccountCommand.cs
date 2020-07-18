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

namespace Firewatch.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountCommand : PersonScopedAuthorizationRequiredRequest, IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly ILogger<DeleteAccountHandler> _logger;
        private readonly IApplicationDbContext _context;

        public DeleteAccountHandler(ILogger<DeleteAccountHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Account), request.Id);
            }

            _context.Accounts.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
