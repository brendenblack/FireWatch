using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firewatch.Application.Accounts.Queries.GetAccounts;
using Firewatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Firewatch.WebUI.Controllers
{
    [Route("api/accounts")]
    [Authorize]
    public class AccountsController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public AccountsController(ICurrentUserService currentUserService, IMediator mediator)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<AccountsVm>> GetAccounts()
        {
            var query = new GetAccountsQuery
            {
                IncludeTransactions = false,
                OwnerId = _currentUserService.UserId,
                RequestorId = _currentUserService.UserId
            };

            return await _mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<FileResult> GetAccountById(int id)
        {
            throw new NotImplementedException();
            //var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

            //return File(vm.Content, vm.ContentType, vm.FileName);
        }
    }
}
