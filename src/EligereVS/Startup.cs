using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EligereVS
{
    public class Startup
    {
        private string contentRootPath;
        private string evsKeyPath;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            contentRootPath = env.ContentRootPath;
            evsKeyPath = Path.Combine(contentRootPath, "Data/EVSKey/");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddSingleton<PersistentStores>();
            services.AddSingleton<TicketsQueue>();

            services.AddDataProtection()
                .SetApplicationName("Eligere")
                .PersistKeysToFileSystem(new DirectoryInfo(evsKeyPath))
                .DisableAutomaticKeyGeneration(); // Automatic Key generation introduces unwanted behavior

            //services.AddScoped<ClientIpCheckActionFilter>(container =>
            //{
            //    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
            //    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

            //    return new ClientIpCheckActionFilter(
            //        Configuration["AdminSafeList"], logger);
            //});
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
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            // Look https://docs.microsoft.com/en-us/aspnet/core/security/ip-safelist?view=aspnetcore-3.1 for the doc
            //app.UseMiddleware<AdminSafeListMiddleware>(Configuration["AdminSafeList"]);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
