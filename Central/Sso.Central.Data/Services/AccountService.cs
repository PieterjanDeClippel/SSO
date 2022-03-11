using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Sso.Central.Data.Exceptions;
using Sso.Central.Data.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sso.Central.Data.Services
{
    public interface IAccountService
    {
        Task<Dtos.Dtos.User> Register(Dtos.Dtos.User user, string password);
        Task<AuthorizationRequest> Login(string email, string password, string redirectUrl);
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

        public async Task<AuthorizationRequest> Login(string email, string password, string redirectUrl)
        {
            var request = await interaction.GetAuthorizationContextAsync(redirectUrl);
            try
            {
                var user = await accountRepository.Login(email, password);
                var userId = user.Id.ToString();

                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, userId, user.UserName, clientId: request?.Client.ClientId));
                var isUser = new IdentityServerUser(userId)
                {
                    DisplayName = user.UserName,
                    AdditionalClaims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
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
