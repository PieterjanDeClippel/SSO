using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sso.Central.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sso.Central.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IEventService events;
        private readonly IClientStore clientStore;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAccountService accountService;
        private readonly IdentityServer4.Validation.ITokenValidator tokenValidator;
        private readonly IServiceProvider serviceProvider;
        private readonly IdentityServer4.Hosting.IEndpointHandler endpointHandler;
        public AccountController(
            IEventService events,
            IClientStore clientStore,
            IIdentityServerInteractionService interaction,
            IAccountService accountService,
            IdentityServer4.Validation.ITokenValidator tokenValidator,
            IServiceProvider serviceProvider)
        {
            this.events = events;
            this.clientStore = clientStore;
            this.interaction = interaction;
            this.accountService = accountService;
            this.tokenValidator = tokenValidator;
            this.serviceProvider = serviceProvider;
        }

        //[HttpGet("Clients")]
        //public async Task<IActionResult> GetClients()
        //{
        //    //var client = await clientStore.FindClientByIdAsync("SsoApplicationClient");
        //    //client.AccessTokenType = IdentityServer4.Models.AccessTokenType.Jwt;
        //    //await accountService.AddClientSecret("SsoApplicationClient", "qsdfghjklm", "new secret");
        //    //var type = typeof(IdentityServer4.IdentityServerConstants).Assembly.DefinedTypes.FirstOrDefault(t => t.Name == "BearerTokenUsageValidator");
        //    ////var services = serviceProvider.serv();
        //    //var method = serviceProvider.GetType().GetMethod(nameof(serviceProvider.GetService), 1, new Type[0])
        //    //    .MakeGenericMethod(type);
        //    //var bearerTokenUsageValidator = method.Invoke(serviceProvider, null);

        //    var result = await tokenValidator.ValidateAccessTokenAsync(Request.Headers["Authorization"], "weatherforecasts.read");
        //    return Ok();
        //}

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            //var user = await accountService.Register(new Dtos.Dtos.User
            //{
            //    UserName = "Pieterjan",
            //    Email = "pieterjandeclippel@msn.com",
            //}, "Aze123@!");
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);

            return View(new ViewModels.Account.LoginVM
            {
                ReturnUrl = returnUrl,
                User = new Dtos.Dtos.User
                {
                    UserName = context?.LoginHint,
                }
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginPost([FromForm] ViewModels.Account.LoginVM model)
        {
            try
            {
                var request = await accountService.Login(model.User.Email, model.Password, model.ReturnUrl);
                if (request != null)
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
            catch (System.Exception ex)
            {
                return View(model);
            }
        }
    }
}
