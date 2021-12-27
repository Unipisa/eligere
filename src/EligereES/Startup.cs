using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using EligereES.Models;
using EligereES.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace EligereES
{

    public class Startup
    {
        private string contentRootPath;
        private string evsKeyPath;
        private string defaultProvider;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            contentRootPath = env.ContentRootPath;
            evsKeyPath = Path.Combine(contentRootPath, "Data/EVSKey/");
            defaultProvider = configuration.GetValue(typeof(string), "DefaultAuthProvider") as string;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PersistentCommissionManager>();
            services.AddSingleton<DownloadOTPManager>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            //.AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddDbContext<ESDB>(o => {
                o.UseSqlServer(Configuration.GetConnectionString("ESDB"));
            });

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, opt =>
            {
                var onTokenValidated = opt.Events.OnTokenValidated;
                opt.Events.OnTokenValidated = (
                async ctxt =>
                {
                    var opt = new DbContextOptionsBuilder<ESDB>();
                    
                    using (var esdb = new ESDB(opt.UseSqlServer(Configuration.GetConnectionString("ESDB")).Options))
                    {
                        onTokenValidated?.Invoke(ctxt);
                        var roles = await EligereRoles.ComputeRoles(esdb, defaultProvider, ctxt.Principal.Identity.Name);
                        var claims = new List<Claim>();
                        roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
                        var appIdentity = new ClaimsIdentity(claims, "EligereIdentity");
                        ctxt.Principal.AddIdentity(appIdentity);

                    }
                });
            });

            services.AddDataProtection()
               .SetApplicationName("Eligere")
               .PersistKeysToFileSystem(new DirectoryInfo(evsKeyPath));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
            IdentityModelEventSource.ShowPII = true;
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

            var supportedCultures = new[] { "en-US", "it" };

            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

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
