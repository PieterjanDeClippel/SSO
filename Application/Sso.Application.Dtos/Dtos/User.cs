using System;

namespace Sso.Application.Dtos.Dtos
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Bypass2faForExternalLogin { get; set; }
    }
}
