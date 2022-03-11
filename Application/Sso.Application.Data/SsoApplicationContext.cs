using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Sso.Application.Data
{
    // dotnet ef migrations add AddIdentity --project ..\Sso.Application.Data --context SsoApplicationContext
    // dotnet ef database update --project ..\Sso.Application.Data --context SsoApplicationContext

    internal class SsoApplicationContext : IdentityDbContext<Entities.User, Entities.Role, Guid>
    {
        #region Constructor
        private readonly IConfiguration configuration;
        public SsoApplicationContext()
        {
            configuration = null;
        }
        public SsoApplicationContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (configuration == null)
            {
                // Only used when generating migrations
                var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SsoApplication;Trusted_Connection=True;ConnectRetryCount=0";
                optionsBuilder.UseSqlServer(migrationsConnectionString, options => {
                    options.MigrationsAssembly(typeof(SsoApplicationContext).Assembly.FullName);
                });
            }
            else
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SsoApplication"));
            }
        }
    }
}
