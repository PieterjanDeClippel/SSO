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

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            return base.BuildChallengeUrl(properties, redirectUri);
        }

        protected override Task<object> CreateEventsAsync()
        {
            return base.CreateEventsAsync();
        }

        protected override string FormatScope()
        {
            return base.FormatScope();
        }

        protected override string FormatScope(IEnumerable<string> scopes)
        {
            return base.FormatScope(scopes);
        }

        protected override void GenerateCorrelationId(AuthenticationProperties properties)
        {
            base.GenerateCorrelationId(properties);
        }

        protected override Task<HandleRequestResult> HandleAccessDeniedErrorAsync(AuthenticationProperties properties)
        {
            return base.HandleAccessDeniedErrorAsync(properties);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            return base.HandleRemoteAuthenticateAsync();
        }

        public override Task<bool> HandleRequestAsync()
        {
            return base.HandleRequestAsync();
        }

        protected override Task InitializeEventsAsync()
        {
            return base.InitializeEventsAsync();
        }

        protected override Task InitializeHandlerAsync()
        {
            return base.InitializeHandlerAsync();
        }

        protected override string ResolveTarget(string scheme)
        {
            return base.ResolveTarget(scheme);
        }

        public override Task<bool> ShouldHandleRequestAsync()
        {
            return base.ShouldHandleRequestAsync();
        }

        protected override bool ValidateCorrelationId(AuthenticationProperties properties)
        {
            return base.ValidateCorrelationId(properties);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Facebook user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            return base.ExchangeCodeAsync(context);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return base.HandleAuthenticateAsync();
        }
    }
}
