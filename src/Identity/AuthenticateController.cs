//using Identity.Models;
//using IdentityModel;
//using IdentityServer4;
//using IdentityServer4.Events;
//using IdentityServer4.Extensions;
//using IdentityServer4.Models;
//using IdentityServer4.Services;
//using IdentityServer4.Stores;
//using IdentityServerHost.Quickstart.UI;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Identity
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [SecurityHeaders]
//    [AllowAnonymous]
//    public class AuthenticateController : Controller
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IIdentityServerInteractionService _interaction;
//        private readonly IClientStore _clientStore;
//        private readonly IAuthenticationSchemeProvider _schemeProvider;
//        private readonly IEventService _events;

//        public AuthenticateController(
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            IIdentityServerInteractionService interaction,
//            IClientStore clientStore,
//            IAuthenticationSchemeProvider schemeProvider,
//            IEventService events)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _interaction = interaction;
//            _clientStore = clientStore;
//            _schemeProvider = schemeProvider;
//            _events = events;
//        }

//        public class LoginRequest
//        {
//            public string Username { get; set; }
//            public string Password { get; set; }
//            public string ReturnUrl { get; set; }

//            public bool RememberLogin { get; set; }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginRequest model)
//        {
//            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

//            var user = await _userManager.FindByNameAsync(model.Username);
//            //await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id, user.Username, clientId: context?.Client.ClientId));

//                // only set explicit expiration here if user chooses "remember me". 
//                // otherwise we rely upon expiration configured in cookie middleware.
//                AuthenticationProperties props = null;
//                if (AccountOptions.AllowRememberLogin && model.RememberLogin)
//                {
//                    props = new AuthenticationProperties
//                    {
//                        IsPersistent = true,
//                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
//                    };
//                };

//                // issue authentication cookie with subject ID and username
//                //var isuser = new IdentityServerUser(user.SubjectId)
//            //    {
//            //        DisplayName = user.Username
//            //    };

//            //    if (ModelState.IsValid)
//            //{
//            //    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
//            //    if (result.Succeeded)
//            //    {

//            //    }
//            //}

//            //AuthenticationProperties props = null;
//            //if (AccountOptions.AllowRememberLogin && model.RememberLogin)
//            //{
//            //    props = new AuthenticationProperties
//            //    {
//            //        IsPersistent = true,
//            //        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
//            //    };
//            //};

//            //// issue authentication cookie with subject ID and username
//            //var issuer = new IdentityServerUser(user.SubjectId)
//            //{
//            //    DisplayName = user.Username
//            //};

//            //await HttpContext.SignInAsync(isuser, props);
//            //if (user != null && context != null)
//            //{
//            //    await HttpContext.SignInAsync(user.SubjectId, user.Username);
//            //    return new JsonResult(new { RedirectUrl = request.ReturnUrl, IsOk = true });
//            //}

//            return Unauthorized();
//        }

//        [HttpGet]
//        [Route("Logout")]
//        public async Task<IActionResult> Logout(string logoutId)
//        {
//            var context = await _interaction.GetLogoutContextAsync(logoutId);
//            bool showSignoutPrompt = true;

//            if (context?.ShowSignoutPrompt == false)
//            {
//                // it's safe to automatically sign-out
//                showSignoutPrompt = false;
//            }

//            if (User?.Identity.IsAuthenticated == true)
//            {
//                // delete local authentication cookie
//                await HttpContext.SignOutAsync();
//            }

//            // no external signout supported for now (see \Quickstart\Account\AccountController.cs TriggerExternalSignout)
//            return Ok(new
//            {
//                showSignoutPrompt,
//                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
//                context?.PostLogoutRedirectUri,
//                context?.SignOutIFrameUrl,
//                logoutId
//            });
//        }
//    }
//}
