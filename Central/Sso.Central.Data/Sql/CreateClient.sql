INSERT INTO [dbo].[Clients] (
	[Enabled], [ClientId], [ProtocolType], [RequireClientSecret], [ClientName], [Description], [RequireConsent], [AllowRememberConsent],
	[AlwaysIncludeUserClaimsInIdToken], [RequirePkce], [AllowPlainTextPkce], [RequireRequestObject], [AllowAccessTokensViaBrowser],
	[FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [BackChannelLogoutSessionRequired],
	[AllowOfflineAccess], [IdentityTokenLifetime], [AllowedIdentityTokenSigningAlgorithms], [AccessTokenLifetime], [AuthorizationCodeLifetime],
	[ConsentLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh],
	[RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IncludeJwtId], [AlwaysSendClientClaims], [ClientClaimsPrefix], [Created],
	[UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [NonEditable])
VALUES (
	'True', 'SsoApplicationClient', 'oidc', 'True', 'SsoApplicationClient', 'My SSO client', 'False', 'True',
	'True', 'True', 'True', 'False', 'True',
	NULL, 'False', NULL, 'False',
	'True', 86400, NULL, 86400, 86400,
	86400, 86400, 86400, 1, 'True',
	86400, 1, 'True', 'True', 'True', 'central-', '2021-11-11 00:00:00',
	86400, 'bearer', 86400, 'False');

DECLARE @ClientId int = @@IDENTITY;

INSERT INTO [dbo].[ClientSecret]
	([Description], [Value], [Type], [ClientId], [Created])
VALUES (
	'My OAuth client', 'e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a', 'SharedSecret', @ClientId, GETDATE()); -- ABC123

INSERT INTO [dbo].[ClientGrantType]
	([GrantType], [ClientId])
VALUES
	('authorization_code', @ClientId),
	('client_credentials', @ClientId);
	
INSERT INTO [dbo].[ClientRedirectUri]
	([RedirectUri], [ClientId])
VALUES
	('https://localhost:44384/signin-central', @ClientId);

INSERT INTO [dbo].[ClientScope]
	([Scope], [ClientId])
VALUES
	('weatherforecasts.read', @ClientId),
	('weatherforecasts.write', @ClientId),
	('openid', @ClientId),
	('profile', @ClientId),
	('email', @ClientId);
	
INSERT INTO [dbo].[ApiResources]
	([Enabled], [Name], [DisplayName], [Description], [ShowInDiscoveryDocument], [Created], [NonEditable])
VALUES
	('True', 'weatherforecasts', 'Weather Forecasts', 'The Weather Forecasts', 'True', '2021/11/15', 'False');
	
DECLARE @ApiResourceId int = @@IDENTITY;

INSERT INTO [dbo].[ApiResourceScope]
	([Scope], [ApiResourceId])
VALUES
	('weatherforecasts.read', @ApiResourceId),
	('weatherforecasts.write', @ApiResourceId);
	
--DECLARE @ClientId int = 1;
INSERT INTO [dbo].[ApiScopes]
	([Enabled], [Name], [DisplayName], [Description], [Emphasize], [Required], [ShowInDiscoveryDocument])
VALUES
	('True', 'weatherforecasts.read', 'WeatherForecasts.Read', 'Get the weather forecasts', 'True', 'False', 'True'),
	('True', 'weatherforecasts.write', 'WeatherForecasts.Write', 'Edit the weather forecasts', 'True', 'False', 'True');

INSERT INTO [dbo].[IdentityResource]
	([Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [NonEditable])
VALUES
	('True', 'openid', 'OpenID', 'Your user identifier', 'False', 'True', 'True', '2021-11-15 00:00:00', 'False'),
	('True', 'profile', 'User profile', 'Your user profile information (first name, last name, etc.)', 'False', 'True', 'True', '2021-11-15 00:00:00', 'False'),
	('True', 'email', 'Your email address', 'Your email address', 'False', 'True', 'True', '2021-11-15 00:00:00', 'False');

INSERT INTO [dbo].[IdentityResource]
	([Enabled], [Name], [DisplayName], [Description], [Required], [Emphasize], [ShowInDiscoveryDocument], [Created], [NonEditable])
VALUES
	('True', 'role', 'Application role', 'Your role in this application', 'False', 'True', 'True', '2021-11-15 00:00:00', 'False')

DECLARE @IdentiyResourceId int = @@IDENTITY;

INSERT INTO [dbo].[IdentityResourceClaim]
	([Type], [IdentityResourceId])
VALUES
	('role', @IdentiyResourceId);

