using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Firewatch.WebUI.Controllers
{
    [Route("api/accounts")]
    [Authorize]
    public class AccountsController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<FileResult> Get(int id)
        {
            throw new NotImplementedException();
            //var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

            //return File(vm.Content, vm.ContentType, vm.FileName);
        }
    }
}
