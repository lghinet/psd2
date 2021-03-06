using bt;
using ing;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using psd2.web.Data;

namespace psd2.web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sso")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var ingConfig = Configuration.GetSection("BankProviders:Ing").Get<IngConfig>();
            services.Configure<IngConfig>(Configuration.GetSection("BankProviders:Ing"));
            services.AddSingleton<IPostConfigureOptions<IngOAuthOptions>, PostConfigureIngOptions>();

            services.AddAuthentication()
                //.AddGoogle()
                .AddOAuth<BtOAuthOptions, OAuthHandler<BtOAuthOptions>>("BT", "BT", options =>
                {
                    options.ClientId = Configuration.GetValue<string>("BankProviders:Bt:ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("BankProviders:Bt:ClientSecret");
                    options.UsePkce = true;
                    options.Scope.Add("AIS:273c540e6c534b1f8d873baf23728969");
                    //options.Scope.Add("PIISP:consentId");
                    //options.Scope.Add("PIS:paymentId");
                })
                .AddOAuth<IngOAuthOptions, IngOAuthHandler>("ING", "ING", options =>
                {
                    options.AuthorizationEndpoint = ingConfig.AuthorizationEndpoint;
                    options.TokenEndpoint = ingConfig.TokenEndpoint;
                    options.CallbackPath = "/signin-ing";
                    options.ClientId = ingConfig.ClientId;
                    options.ClientSecret = "fake";
                    options.Scope.Add("payment-accounts:balances:view");
                    options.Scope.Add("payment-accounts:transactions:view");
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

            services.AddHttpClient("ING")
                .ConfigureHttpClient((sp, client) => { client.DefaultRequestHeaders.Add("keyId", "5ca1ab1e-c0ca-c01a-cafe-154deadbea75"); })
                .ConfigurePrimaryHttpMessageHandler(sp => Utils.GetSecuredHandler(ingConfig.ClientCertificate))
                .AddHttpMessageHandler(sp => new DigestHttpHandler(Utils.CreateCertificate(ingConfig.SingingCertificate)))
                .AddUserAccessTokenHandler();

            services.AddAccessTokenManagement(options => options.User.Scheme = "ING");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
        }
    }
}