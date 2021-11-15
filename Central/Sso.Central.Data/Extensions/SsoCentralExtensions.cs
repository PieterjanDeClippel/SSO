using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sso.Central.Data.Extensions
{
    public static class SsoCentralExtensions
    {
        public static IServiceCollection AddSsoCentral(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<SsoCentralContext>()
                .AddScoped<Repositories.IAccountRepository, Repositories.AccountRepository>()
                .AddScoped<Services.IAccountService, Services.AccountService>();

            services.AddIdentity<Entities.User, Entities.Role>()
                .AddEntityFrameworkStores<SsoCentralContext>();

            var isBuilder = services.AddIdentityServer();
                //.AddInMemoryClients(new List<Client>
                //{
                //    new Client
                //    {
                //        ClientId = "SsoApplicationClient",
                //        ClientName = "SsoApplicationClient",
                //        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                //        Enabled = true,
                //        ClientSecrets = new List<Secret>
                //        {
                //            new Secret("SuperSecretPassword".Sha256())
                //        },
                //        RequireClientSecret = false,
                //        RequirePkce = false,

                //        AllowedScopes = new List<string>
                //        {
                //            IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                //            IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                //            IdentityServer4.IdentityServerConstants.StandardScopes.Email,
                //            "weatherforecasts.read"
                //        },
                //        RedirectUris = new List<string>
                //        {
                //            "https://localhost:44384/signin-central",
                //        },
                //        Claims = new List<IdentityServer4.Models.ClientClaim>()
                //    }
                //})
                //.AddInMemoryIdentityResources(new List<IdentityResource>
                //{
                //    new IdentityResources.OpenId(),
                //    new IdentityResources.Profile(),
                //    new IdentityResources.Email(),
                //    new IdentityResource
                //    {
                //        Name = "role",
                //        UserClaims = new List<string> {"role"}
                //    }
                //})
                //.AddInMemoryApiResources(new List<ApiResource>
                //{
                //    new ApiResource
                //    {
                //        Name = "weatherforecasts",
                //        DisplayName = "Weatherforecasts API",
                //        Description = "Allow the application to access Weatherforecasts on your behalf",
                //        Scopes = new List<string> { "weatherforecasts.read", "weatherforecasts.write"},
                //        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                //        UserClaims = new List<string> {"role"}
                //    }
                //})
                //.AddInMemoryApiScopes(new List<ApiScope>
                //{
                //    new ApiScope("weatherforecasts.read", "Read Access to Weatherforecasts API"),
                //    new ApiScope("weatherforecasts.write", "Write Access to Weatherforecasts API")
                //})
                //.AddTestUsers(new List<IdentityServer4.Test.TestUser>
                //{
                //    new IdentityServer4.Test.TestUser
                //    {
                //        SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                //        Username = "Pieterjan",
                //        Password = "password",
                //        Claims = new List<System.Security.Claims.Claim> {
                //            new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Email, "pieterjan@example.com"),
                //            new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Role, "admin")
                //        }
                //    }
                //})
            isBuilder
                .AddDeveloperSigningCredential()
                .AddOperationalStore<SsoCentralContext>(identityServerOptions =>
                {
                    //if (configuration == null)
                    //{
                    //    var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SsoCentral;Trusted_Connection=True;ConnectRetryCount=0";
                    //    identityServerOptions.ConfigureDbContext = (builder) => builder
                    //        .UseSqlServer(migrationsConnectionString, options => {
                    //            options.MigrationsAssembly(typeof(SsoCentralContext).Assembly.FullName);
                    //        });
                    //}
                    //else
                    //{
                    //    identityServerOptions.ConfigureDbContext = (builder) => builder
                    //        .UseSqlServer(configuration.GetConnectionString("SsoCentral"));
                    //}
                    identityServerOptions.ConfigureDbContext = (builder) => builder
                        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SsoCentral;Trusted_Connection=True;ConnectRetryCount=0");
                })
                .AddConfigurationStore<SsoCentralContext>(identityServerOptions =>
                //{
                //    if (configuration == null)
                //    {
                //        var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SsoCentral;Trusted_Connection=True;ConnectRetryCount=0";
                //        identityServerOptions.ConfigureDbContext = (builder) => builder
                //            .UseSqlServer(migrationsConnectionString, options => {
                //                options.MigrationsAssembly(typeof(SsoCentralContext).Assembly.FullName);
                //            });
                //    }
                //    else
                //    {
                //        identityServerOptions.ConfigureDbContext = (builder) => builder
                //            .UseSqlServer(configuration.GetConnectionString("SsoCentral"));
                //    }
                        identityServerOptions.ConfigureDbContext = (builder) => builder
                            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SsoCentral;Trusted_Connection=True;ConnectRetryCount=0")
                )
                .AddAspNetIdentity<Entities.User>();

            return services;
        }
    }
}
