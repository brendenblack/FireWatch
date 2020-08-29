using Firewatch.Application.Common.Interfaces;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Common.Behaviours
{
    public class UserInitializationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly INewUserService _newUserService;
        private readonly IApplicationDbContext _context;

        public UserInitializationBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, INewUserService newUserService, IApplicationDbContext context)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _newUserService = newUserService;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            
            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
            {
                // if there is no user associated with the context of this request, no-op
                return await next();
            }

            if (_context.People.Any(p => p.Id == _currentUserService.UserId))
            {
                return await next();
            }

            var result = await _newUserService.InitializeNewUser(_currentUserService.UserId);

            if (!result.IsSuccess)
            {
                throw new Exception("");
            }

            return await next();
        }
    }
}
