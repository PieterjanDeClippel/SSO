﻿using IdentityServer4.Events;
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

                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName, clientId: request?.Client.ClientId));
                await httpContextAccessor.HttpContext.SignInAsync(new IdentityServer4.IdentityServerUser(user.Id.ToString()) { DisplayName = user.UserName });

                return request;
            }
            catch (LoginException loginEx)
            {
                await events.RaiseAsync(new UserLoginFailureEvent(loginEx.Email, "invalid credentials", clientId: request?.Client.ClientId));
                throw loginEx;
            }
        }
    }
}