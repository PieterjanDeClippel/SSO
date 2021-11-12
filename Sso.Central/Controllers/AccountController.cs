using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sso.Central.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IClientStore clientStore;

        //private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            IClientStore clientStore,
            //UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.interaction = interaction;
            this.events = events;
            this.clientStore = clientStore;
            //this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);

            return View(new ViewModels.Account.LoginVM
            {
                ReturnUrl = returnUrl,
                Username = context?.LoginHint
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginPost([FromForm] ViewModels.Account.LoginVM model)
        {
            var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            try
            {
                var user = await signInManager.UserManager.FindByNameAsync(model.Username);
                if (user == null) throw new System.Exception();

                var signinResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                if (!signinResult.Succeeded) throw new System.Exception();

                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                await HttpContext.SignInAsync(new IdentityServer4.IdentityServerUser(user.Id) { DisplayName = user.UserName });

                if (context != null)
                {
                    return Redirect(model.ReturnUrl);
                }
                else if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/");
                }
                else
                {
                    throw new System.Exception("Invalid return url");
                }
            }
            catch (System.Exception)
            {
                await events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                return View(model);
            }
        }
    }
}
