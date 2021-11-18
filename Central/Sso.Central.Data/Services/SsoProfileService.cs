using IdentityServer4.Models;
using IdentityServer4.Services;
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

        //IdentityServer4.EntityFramework.Entities.IdentityResourceClaim

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            var allClaims = await userManager.GetClaimsAsync(user);
            //var filteredClaims = allClaims.Where(c => context.RequestedClaimTypes.Contains(c.Type));
            //context.IssuedClaims = filteredClaims.ToList();
            //allClaims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName));
            //allClaims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email));
            //allClaims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.MobilePhone, user.PhoneNumber));
            context.IssuedClaims = allClaims.ToList();
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
