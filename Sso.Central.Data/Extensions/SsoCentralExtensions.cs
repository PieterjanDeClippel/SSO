using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sso.Central.Data.Extensions
{
    public static class SsoCentralExtensions
    {
        public static IServiceCollection AddSsoCentral(this IServiceCollection services)
        {
            services
                .AddDbContext<SsoCentralContext>();
            //.AddScoped<Repositories.IAccountRepository, Repositories.AccountRepository>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<SsoCentralContext>();

            var isb = services.AddIdentityServer();
            isb
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId = "oauthClient",
                        ClientName = "oauthClient",
                        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                        Enabled = true,
                        ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                        AllowedScopes = new List<string> {"weatherforecasts.read"},
                        RedirectUris = new List<string>
                        {
                            "https://localhost:44375/signin-central"
                        },
                    }
                })
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResource
                    {
                        Name = "role",
                        UserClaims = new List<string> {"role"}
                    }
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource
                    {
                        Name = "api1",
                        DisplayName = "API #1",
                        Description = "Allow the application to access API #1 on your behalf",
                        Scopes = new List<string> { "weatherforecasts.read", "weatherforecasts.write"},
                        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                        UserClaims = new List<string> {"role"}
                    }
                })
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                    new ApiScope("weatherforecasts.read", "Read Access to API #1"),
                    new ApiScope("weatherforecasts.write", "Write Access to API #1")
                })
                .AddTestUsers(new List<IdentityServer4.Test.TestUser>
                {
                    new IdentityServer4.Test.TestUser
                    {
                        SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                        Username = "Pieterjan",
                        Password = "password",
                        Claims = new List<System.Security.Claims.Claim> {
                            new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Email, "pieterjan@example.com"),
                            new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Role, "admin")
                        }
                    }
                })
                .AddDeveloperSigningCredential();

            isb
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = (builder) => builder.UseInMemoryDatabase("SsoCentral");
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = (builder) => builder.UseInMemoryDatabase("SsoCentral");
                });
            isb.AddAspNetIdentity<IdentityUser>();

            return services;
        }
    }
}
