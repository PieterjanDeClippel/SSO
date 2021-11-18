using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Sso.Application
{
    public class CentralOptions : OAuthOptions
    {
        public CentralOptions()
        {
            ClaimsIssuer = "https://localhost:44359";
            CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-central");
            AuthorizationEndpoint = "https://localhost:44359/connect/authorize";
            TokenEndpoint = "https://localhost:44359/connect/token";
            UserInformationEndpoint = "https://localhost:44359/connect/userinfo";

            Scope.Add("openid");
            Scope.Add("email");
            Scope.Add("phone");

            UsePkce = true;

            ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.NameIdentifier, "sub");
            ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");
        }
    }
}
