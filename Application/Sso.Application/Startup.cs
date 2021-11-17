using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sso.Application.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sso.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    //options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "central";
                })
                //.AddCookie(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, options =>
                .AddCookie("cookie", options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/AccessDenied");
                })
                .AddOAuth<CentralOptions, CentralHandler>("central", options =>
                {
                    options.ClaimsIssuer = "https://localhost:44359";
                    options.SaveTokens = true;
                    options.ClientId = "SsoApplicationClient";
                    options.ClientSecret = "qsdfghjklm";
                    options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-central");

                    options.Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    options.Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                    options.Scope.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("role");
                    options.Scope.Add("weatherforecasts.read");
                    options.Scope.Add("weatherforecasts.write");

                    options.UsePkce = true;

                    options.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "sub");
                    options.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name");
                    options.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "email");
                    //options.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/phone", "phone");

                    //options.ClaimActions.MapJsonKey("sub", "sub");
                    //options.ClaimActions.MapJsonKey("name", "name");
                    //options.ClaimActions.MapJsonKey("email", "email");
                    //options.ClaimActions.MapJsonKey("phone", "phone");
                });
            //.AddOpenIdConnect("central", options =>
            //{
            //    //options.SignInScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.SignInScheme = "cookie";

            //    options.Authority = "https://localhost:44359";
            //    options.RequireHttpsMetadata = false;
            //    options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-central");

            //    options.ClientId = "SsoApplicationClient";
            //    options.ClientSecret = "qsdfghjklm";
            //    options.ResponseType = "code";
            //    options.UsePkce = true;
            //    //options.ResponseType = "code id_token";

            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;

            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.Scope.Add("weatherforecasts.read");
            //    options.Scope.Add("weatherforecasts.write");

            //    //options.ClaimActions.MapJsonKey("website", "website");

            //    options.Events.OnUserInformationReceived = (info) =>
            //    {
            //        return Task.CompletedTask;
            //    };
            //});

            services.AddSsoApplication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
