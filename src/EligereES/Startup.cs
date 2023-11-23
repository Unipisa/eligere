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
using Microsoft.AspNetCore.Authentication.OAuth;
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

using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EligereES
{

    public class Startup
    {
        private string contentRootPath;
        private string evsKeyPath;
        private string defaultProvider;
        private ILogger<Startup> _logger;


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
            services.AddTransient<IClaimsTransformation, EligereClaimsTransformation>();
            services.AddSingleton<PersistentCommissionManager>();
            services.AddSingleton<DownloadOTPManager>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            if (Configuration.GetValue<bool?>("Spid:Enabled") ?? false)
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddOAuth("Spid", options =>
                    {
                        options.AuthorizationEndpoint = Configuration["Spid:AuthorizationEndpoint"];
                        options.TokenEndpoint = Configuration["Spid:TokenEndpoint"];
                        options.UserInformationEndpoint = Configuration["Spid:UserInformationEndpoint"];
                        options.CallbackPath = "/spid-signin";
                        options.ClientId = Configuration["Spid:OAuth:ClientId"];
                        options.ClientSecret = Configuration["Spid:OAuth:ClientSecret"];

                        options.Scope.Add("openid");

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                                var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                                response.EnsureSuccessStatusCode();
                                var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                                var gn = json.RootElement.GetProperty("given_name").GetString();
                                var ln = json.RootElement.GetProperty("family_name").GetString();
                                var fid = json.RootElement.GetProperty("fiscalNumber").GetString();
                                var email = json.RootElement.GetProperty("email").GetString();
                                context.Identity.AddClaim(new Claim(ClaimTypes.GivenName, gn, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                context.Identity.AddClaim(new Claim(ClaimTypes.Surname, ln, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                context.Identity.AddClaim(new Claim(ClaimTypes.Name, $"{gn} {ln}", ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, fid, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                            }
                        };
                    });
            }

            //.AddAzureAD(options => Configuration.Bind("AzureAd", options));


            services.AddDbContext<ESDB>(o => {
                o.UseSqlServer(Configuration.GetConnectionString("ESDB"));
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

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
