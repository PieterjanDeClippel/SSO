using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Sso.Application.Data.Mappers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sso.Application.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<Dtos.Dtos.ExternalLoginResult> PerformExternalLogin();
    }
    internal class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<Entities.User> signInManager;
        private readonly UserManager<Entities.User> userManager;
        private readonly IUserMapper userMapper;
        public AccountRepository(
            SignInManager<Entities.User> signInManager,
            UserManager<Entities.User> userManager,
            IUserMapper userMapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userMapper = userMapper;
        }

        public Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Task.FromResult(properties);
        }

        public async Task<Dtos.Dtos.ExternalLoginResult> PerformExternalLogin()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null) throw new UnauthorizedAccessException();

            var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                var new_user = new Entities.User
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    EmailConfirmed = true,
                    PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                    PhoneNumberConfirmed = true,
                };

                var strDateOfBirth = info.Principal.FindFirstValue(ClaimTypes.DateOfBirth);
                if (DateTime.TryParse(strDateOfBirth, out var dateOfBirth)) new_user.DateOfBirth = dateOfBirth;

                var id_result = await userManager.CreateAsync(new_user);
                if (id_result.Succeeded)
                {
                    user = new_user;
                }
                else
                {
                    // User creation failed, probably because the email address is already present in the database
                    if (id_result.Errors.Any(e => e.Code == "DuplicateEmail"))
                    {
                        var existing = await userManager.FindByEmailAsync(new_user.Email);
                        var existing_logins = await userManager.GetLoginsAsync(existing);

                        if (existing_logins.Any())
                        {
                            throw new Exceptions.OtherAccountException(existing_logins);
                        }
                        else
                        {
                            throw new Exception("Could not create account from social profile");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not create account from social profile");
                    }
                }

                await userManager.AddLoginAsync(user, new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));
            }

            var signinResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, user.Bypass2faForExternalLogin);

            if (signinResult.Succeeded)
            {
                return new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Success,
                    Provider = info.LoginProvider,
                    User = await userMapper.Entity2Dto(user),
                };
            }
            else if (signinResult.RequiresTwoFactor)
            {
                return new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.RequiresTwoFactor,
                    Provider = info.LoginProvider,
                    User = await userMapper.Entity2Dto(user),
                };
            }
            else
            {
                return new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Failed,
                };
            }
        }
    }
}
