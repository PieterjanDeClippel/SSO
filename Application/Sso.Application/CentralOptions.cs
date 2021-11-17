using Microsoft.AspNetCore.Authentication;
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

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");
            //Scope.Add(System.Security.Claims.ClaimTypes.NameIdentifier);
            Scope.Add("name");
            Scope.Add("weatherforecasts.read");
            Scope.Add("weatherforecasts.write");

            UsePkce = true;

            ClaimActions.MapJsonKey("sub", "sub"); // Sub claim <= "sub" from json
            ClaimActions.MapJsonKey("name", "name");
            ClaimActions.MapJsonKey("email", "email");
            //ClaimActions.MapJsonKey("name", "nameidentifier");
        }
    }
}
