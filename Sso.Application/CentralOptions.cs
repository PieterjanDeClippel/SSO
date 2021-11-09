using Microsoft.AspNetCore.Authentication.OAuth;

namespace Sso.Application
{
    public class CentralOptions : OAuthOptions
    {
        public CentralOptions()
        {
            CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-central");
            AuthorizationEndpoint = "https://localhost:44359/connect/authorize";
            TokenEndpoint = "https://localhost:44359/connect/token";
            UserInformationEndpoint = "https://localhost:44359/connect/userinfo";

        }
    }
}
