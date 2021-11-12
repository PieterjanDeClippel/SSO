using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Sso.Central.Data
{
    internal class SsoCentralContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SsoCentralContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("SsoCentral");
        }
    }
}
