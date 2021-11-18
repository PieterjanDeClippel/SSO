using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Sso.Central.Data
{
    // dotnet ef migrations add AddIdentity --project ..\Sso.Central.Data --context SsoCentralContext
    // dotnet ef database update --project ..\Sso.Central.Data --context SsoCentralContext


    internal class SsoCentralContext : IdentityDbContext<Entities.User, Entities.Role, Guid>, IPersistedGrantDbContext, IConfigurationDbContext
    {
        #region Constructor
        private readonly IConfiguration configuration;
        public SsoCentralContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SsoCentralContext()
        {
            configuration = null;
        }
        #endregion

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (configuration == null)
            {
                // Only used when generating migrations
                var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SsoCentral;Trusted_Connection=True;ConnectRetryCount=0";
                optionsBuilder.UseSqlServer(migrationsConnectionString, options => {
                    options.MigrationsAssembly(typeof(SsoCentralContext).Assembly.FullName);
                });
            }
            else
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SsoCentral"));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DeviceFlowCodes>().HasNoKey();
            builder.Entity<PersistedGrant>().HasKey(pg => pg.Key);
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await base.SaveChangesAsync();
            return result;
        }
    }
}
