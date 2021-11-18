using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
        public AccountRepository(
            SignInManager<Entities.User> signInManager,
            UserManager<Entities.User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                string username = info.Principal.FindFirstValue(ClaimTypes.Name);
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                string mobilePhone = info.Principal.FindFirstValue(ClaimTypes.MobilePhone);

                var new_user = new Entities.User
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = mobilePhone,
                    PhoneNumberConfirmed = true,
                };
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
                        var existing = await userManager.FindByEmailAsync(email);
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
                    User = new Dtos.Dtos.User
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
                    },
                };
            }
            else if (signinResult.RequiresTwoFactor)
            {
                return new Dtos.Dtos.ExternalLoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.RequiresTwoFactor,
                    Provider = info.LoginProvider,
                    User = new Dtos.Dtos.User
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
                    },
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
