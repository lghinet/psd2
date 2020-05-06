using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

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
            services.AddAuthentication(options =>
                {
                   options.DefaultScheme = "psd2";
                   // options.DefaultChallengeScheme = "ING";
                })
                .AddCookie("psd2")
                .AddOAuth<OAuthOptions, IngOAuthHandler>("ING", options =>
                {
                    options.AuthorizationEndpoint = "https://api.sandbox.ing.com/oauth2/authorization-server-url";
                    options.TokenEndpoint = "https://api.sandbox.ing.com/oauth2/token";
                    options.CallbackPath = "/signin-ing";
                    options.ClientId = "e77d776b-90af-4684-bebc-521e5b2614dd"; //"5f6d1bc8-a2f1-47e4-b01a-e04d121672f2";
                    options.ClientSecret = "fake";
                    options.Scope.Add("payment-accounts:balances:view");
                    options.Scope.Add("payment-accounts:transactions:view");
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
