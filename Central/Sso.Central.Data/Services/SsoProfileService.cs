using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Sso.Central.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sso.Central.Data.Services
{
    internal class SsoProfileService : IProfileService
    {
        private readonly UserManager<User> userManager;
        public SsoProfileService(UserManager<Entities.User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            var allClaimsFromDatabase = await userManager.GetClaimsAsync(user);
            context.IssuedClaims = context.RequestedClaimTypes
                .Select(ct =>
                {
                    switch (ct)
                    {
                        case "name":
                            if (string.IsNullOrEmpty(user.UserName)) return null;
                            else return new System.Security.Claims.Claim("name", user.UserName);
                        case "email":
                            if (string.IsNullOrEmpty(user.Email)) return null;
                            else return new System.Security.Claims.Claim("email", user.Email);
                        case "mobilephone":
                            if (string.IsNullOrEmpty(user.PhoneNumber)) return null;
                            else return new System.Security.Claims.Claim("mobilephone", user.PhoneNumber);
                        default:
                            return allClaimsFromDatabase.FirstOrDefault(c => c.Type == ct);
                    }
                })
                .Where(c => c != null)
                .ToList();

            //var filteredClaims = allClaims.Where(c => false/*context.RequestedClaimTypes.Contains(c.Type)*/).ToList();

            //// Optionally add more claims or compute them based on the ClaimsPrincipal properties
            //filteredClaims.Add(new System.Security.Claims.Claim("name", user.UserName));
            //filteredClaims.Add(new System.Security.Claims.Claim("email", user.Email));
            //filteredClaims.Add(new System.Security.Claims.Claim("mobilephone", user.PhoneNumber));

            //context.IssuedClaims = filteredClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            var emailConfirmed = await userManager.IsEmailConfirmedAsync(user);
            var phoneConfirmed = await userManager.IsPhoneNumberConfirmedAsync(user);
            context.IsActive = emailConfirmed || phoneConfirmed;
        }
    }
}
