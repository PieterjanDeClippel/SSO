using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sso.Application
{
    public class CentralHandler : OAuthHandler<CentralOptions>
    {
        public CentralHandler(IOptionsMonitor<CentralOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task InitializeEventsAsync()
        {
            await base.InitializeEventsAsync();
        }

        protected override async Task InitializeHandlerAsync()
        {
            await base.InitializeHandlerAsync();
        }

        protected override string ResolveTarget(string scheme)
        {
            var result = base.ResolveTarget(scheme);
            return result;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
        }

        protected override void GenerateCorrelationId(AuthenticationProperties properties)
        {
            base.GenerateCorrelationId(properties);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var challengeUrl = base.BuildChallengeUrl(properties, redirectUri);
            return challengeUrl;
        }

        protected override string FormatScope()
        {
            var scope = base.FormatScope();
            return scope;
        }

        protected override string FormatScope(IEnumerable<string> scopes)
        {
            var scope = base.FormatScope(scopes);
            return scope;
        }

        // Everything below this line is never called
        protected override async Task<object> CreateEventsAsync()
        {
            var events = await base.CreateEventsAsync();
            return events;
        }

        protected override async Task<HandleRequestResult> HandleAccessDeniedErrorAsync(AuthenticationProperties properties)
        {
            var result = await base.HandleAccessDeniedErrorAsync(properties);
            return result;
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await base.HandleForbiddenAsync(properties);
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var result = await base.HandleRemoteAuthenticateAsync();
            foreach (var action in Options.ClaimActions)
            {
                //action.Run(payload.RootElement, identity, ClaimsIssuer);
            }
            return result;
        }

        public override async Task<bool> HandleRequestAsync()
        {
            var result = await base.HandleRequestAsync();
            return result;
        }

        public override async Task<bool> ShouldHandleRequestAsync()
        {
            var result = await base.ShouldHandleRequestAsync();
            return result;
        }

        protected override bool ValidateCorrelationId(AuthenticationProperties properties)
        {
            var result = base.ValidateCorrelationId(properties);
            return result;
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            //var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            //var response = await Backchannel.GetAsync(Options.UserInformationEndpoint, Context.RequestAborted);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new System.Uri(Options.UserInformationEndpoint),
            };
            request.Headers.Add("Authorization", "Bearer " + tokens.AccessToken); // "Bearer" = Case sensitive
            request.Headers.Add("scope", "openid,email,profile,name,id");
            var response = await Backchannel.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Central user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
            }

            string json = await response.Content.ReadAsStringAsync();
            var payload = JsonDocument.Parse(json);

            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            var result = await base.ExchangeCodeAsync(context);
            return result;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = await base.HandleAuthenticateAsync();
            return result;
        }
    }
}
