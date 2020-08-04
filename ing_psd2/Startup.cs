using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ing_psd2
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddOAuth<OAuthOptions, IngOAuthHandler>("ING", options =>
                {
                    options.AuthorizationEndpoint = "https://api.sandbox.ing.com/oauth2/authorization-server-url";
                    options.TokenEndpoint = "https://api.sandbox.ing.com/oauth2/token";
                    options.CallbackPath = "/signin-ing";
                    options.ClientId = "5ca1ab1e-c0ca-c01a-cafe-154deadbea75";
                    options.ClientSecret = "fake";
                    options.Scope.Add("payment-accounts:balances:view");
                    options.Scope.Add("payment-accounts:transactions:view");
                    options.BackchannelHttpHandler = new IngHttpHandler(options.TokenEndpoint);
                    options.SaveTokens = true;
                    //options.Events.OnCreatingTicket = ctx =>
                    //{
                    //    var tokens = ctx.Properties.GetTokens().ToList();
                    //    ctx.Properties.StoreTokens(tokens);
                    //    return Task.CompletedTask;
                    //};
                });

            services.AddAuthorization();
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}