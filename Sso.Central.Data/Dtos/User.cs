using System;
using System.Collections.Generic;
using System.Text;

namespace Sso.Central.Data.Dtos
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
