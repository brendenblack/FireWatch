using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Firewatch.WebUI.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public class LoginRequest
        {

        }

    }
}
