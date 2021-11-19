using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sso.Application.Data.Entities
{
    internal class User : IdentityUser<Guid>
    {
        public bool Bypass2faForExternalLogin { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
