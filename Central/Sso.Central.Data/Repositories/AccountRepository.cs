using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Sso.Central.Data.Exceptions;
using System.Threading.Tasks;

namespace Sso.Central.Data.Repositories
{
    internal interface IAccountRepository
    {
        Task<Dtos.Dtos.User> Register(Dtos.Dtos.User user, string password);
        Task<Dtos.Dtos.User> Login(string email, string password);
    }

    internal class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Entities.User> userManager;
        private readonly SignInManager<Entities.User> signinManager;
        public AccountRepository(
            UserManager<Entities.User> userManager,
            SignInManager<Entities.User> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        public async Task<Dtos.Dtos.User> Register(Dtos.Dtos.User user, string password)
        {
            var entity = new Entities.User
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            await userManager.CreateAsync(entity, password);
            return new Dtos.Dtos.User
            {
                Id = user.Id,
                Email = entity.Email,
                UserName = entity.UserName,
            };
        }

        public async Task<Dtos.Dtos.User> Login(string email, string password)
        {
            try
            {
                var user = await signinManager.UserManager.FindByEmailAsync(email);
                if (user == null) throw new LoginException { Email = email };

                var signinResult = await signinManager.CheckPasswordSignInAsync(user, password, true);
                if (!signinResult.Succeeded) throw new LoginException { UserId = user.Id, Email = email, Username = user.UserName };

                return new Dtos.Dtos.User
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                };
            }
            catch (LoginException)
            {
                throw;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
