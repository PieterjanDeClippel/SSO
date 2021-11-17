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

            Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");
            Scope.Add("phone");
            Scope.Add("role");
            Scope.Add("weatherforecasts.read");
            Scope.Add("weatherforecasts.write");

            UsePkce = true;

            ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "sub");
            ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name");
            ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "email");
            //ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/phone", "phone");

            //ClaimActions.MapJsonKey("sub", "sub");
            //ClaimActions.MapJsonKey("name", "name");
            //ClaimActions.MapJsonKey("email", "email");
            //ClaimActions.MapJsonKey("phone", "phone");
        }
    }
}
