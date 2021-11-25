using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sso.Application.Data.Repositories;
using Sso.Application.Dtos.Dtos;

namespace Sso.Application.Controllers
{
    [Route("web/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("connect/{provider}")]
        public async Task<IActionResult> ExternalLogin(string provider)
        {
            var redirectUrl = Url.RouteUrl("account-external-connect-callback", new { provider });
            var properties = await accountRepository.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("connect/{provider}/callback", Name = "account-external-connect-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromRoute] string provider)
        {
            try
            {
                var loginResult = await accountRepository.PerformExternalLogin();
                return View(loginResult);
            }
            catch (Data.Exceptions.OtherAccountException otherAccountEx)
            {
                return View(new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Failed,
                    Provider = provider,
                    Error = "Could not login",
                    ErrorDescription = otherAccountEx.Message,
                });
            }
            catch (System.Exception ex)
            {
                return View(new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Failed,
                    Provider = provider,
                    Error = "Could not login",
                    ErrorDescription = "There was an error with your social login"
                });
            }
        }

        [Authorize]
        [HttpGet("current-user", Name = "account-currentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await accountRepository.GetCurrentUser(User);
            return Ok(user);
        }

        [HttpPost("csrf-refresh", Name = "account-csrf-refresh")]
        public async Task<ActionResult> RefreshCsrfToken()
        {
            // Just an empty method that returns a new cookie with a new CSRF token.
            // Call this method when the user has signed in/out.
            await Task.Delay(5);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout", Name = "account-logout")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Logout()
        {
            await accountRepository.Logout();
            return Ok();
        }
    }
}
