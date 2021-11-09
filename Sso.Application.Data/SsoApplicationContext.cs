using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Sso.Application.Data
{
    internal class SsoApplicationContext : IdentityDbContext<Entities.User, Entities.Role, Guid>
    {
        public SsoApplicationContext()
        {
        }
    }
}
