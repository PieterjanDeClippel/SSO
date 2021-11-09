namespace Sso.Application.Data.Dtos
{
    public class ExternalLoginResult
    {
        public Enums.ELoginStatus Status { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string Provider { get; set; }
    }
}
