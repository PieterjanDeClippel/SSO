using System;

namespace Sso.Central.Data.Exceptions
{
    public class LoginException : Exception
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }
    }
}
