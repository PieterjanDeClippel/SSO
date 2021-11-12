namespace Sso.Central.ViewModels.Account
{
    public class LoginVM
    {
        public string ReturnUrl { get; set; }
        public Dtos.Dtos.User User { get; set; }
        public string Password { get; set; }
    }
}
