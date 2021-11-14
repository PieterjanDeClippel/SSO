using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sso.Application.Data.Extensions
{
    public static class SsoApplicationExtensions
    {
        public static IServiceCollection AddSsoApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<Entities.User, Entities.Role>(options =>
                {
                })
                .AddEntityFrameworkStores<SsoApplicationContext>()
                .AddDefaultTokenProviders();

            return services
                .AddDbContext<SsoApplicationContext>()
                .AddScoped<Repositories.IAccountRepository, Repositories.AccountRepository>();
        }
    }
}
