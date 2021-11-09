using Microsoft.AspNetCore.Mvc;
using Sso.Application.Data.Repositories;
using System.Threading.Tasks;

namespace Sso.Application.Controllers
{
    [Route("api/[controller]")]
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
                switch (loginResult.Status)
                {
                    case Data.Enums.ELoginStatus.Success:
                        return Ok(loginResult);
                    case Data.Enums.ELoginStatus.RequiresTwoFactor:
                        return Redirect("/Account/TwoFactor");
                    default:
                        return View(loginResult);
                }
            }
            catch (Data.Exceptions.OtherAccountException otherAccountEx)
            {
                return View(new Data.Dtos.ExternalLoginResult
                {
                    Status = Data.Enums.ELoginStatus.Failed,
                    Provider = provider,
                    Error = "Could not login",
                    ErrorDescription = otherAccountEx.Message,
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
