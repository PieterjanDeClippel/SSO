using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Sso.Central.Data.Exceptions;
using Sso.Central.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sso.Central.Data.Services
{
    public interface IAccountService
    {
        Task<Dtos.Dtos.User> Register(Dtos.Dtos.User user, string password);
        Task<IdentityServer4.Models.AuthorizationRequest> Login(string email, string password, string redirectUrl);
        //Task AddClientSecret(string clientId, string secret, string description);
    }

    internal class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IEventService events;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IIdentityServerInteractionService interaction;
        public AccountService(
            IAccountRepository accountRepository,
            IEventService events,
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerInteractionService interaction)
        {
            this.accountRepository = accountRepository;
            this.events = events;
            this.httpContextAccessor = httpContextAccessor;
            this.interaction = interaction;
        }

        public async Task<Dtos.Dtos.User> Register(Dtos.Dtos.User user, string password)
        {
            var newUser = await accountRepository.Register(user, password);
            return newUser;
        }

        public async Task<IdentityServer4.Models.AuthorizationRequest> Login(string email, string password, string redirectUrl)
        {
            var request = await interaction.GetAuthorizationContextAsync(redirectUrl);
            try
            {
                var user = await accountRepository.Login(email, password);

                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: request?.Client.ClientId));
                var isUser = new IdentityServer4.IdentityServerUser(user.Id)
                {
                    DisplayName = user.UserName,
                    AdditionalClaims = new[]
                    {
                        //new System.Security.Claims.Claim("sub", user.Id),
                        //new System.Security.Claims.Claim("email", user.Email),
                        //new System.Security.Claims.Claim("profile", user.UserName),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                    }
                };
                await httpContextAccessor.HttpContext.SignInAsync(isUser);

                return request;
            }
            catch (LoginException loginEx)
            {
                await events.RaiseAsync(new UserLoginFailureEvent(loginEx.Email, "invalid credentials", clientId: request?.Client.ClientId));
                throw loginEx;
            }
        }

        //public async Task AddClientSecret(string clientId, string secret, string description)
        //{
        //    await accountRepository.AddClientSecret(clientId, secret, description);
        //}
    }
}
