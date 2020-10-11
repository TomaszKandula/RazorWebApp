using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using SecureWebApp.Logic;
using SecureWebApp.Helpers;
using SecureWebApp.AppLogger;
using SecureWebApp.Models.Database;

namespace SecureWebApp
{

    public class Startup
    {

        private readonly IConfiguration FConfiguration;

        public Startup(IConfiguration AConfiguration) 
        {
            FConfiguration = AConfiguration;
        }

        public void ConfigureServices(IServiceCollection AServices)
        {

            AServices.AddMvc(AOption => AOption.CacheProfiles
                .Add("ResponseCache", new CacheProfile
                {
                    Duration = 5,
                    Location = ResponseCacheLocation.Any,
                    NoStore = false
                }));

            AServices.AddMvc(AOption => AOption.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            AServices.AddRazorPages().AddRazorRuntimeCompilation();
            AServices.AddControllers();

            AServices.AddSession(AOptions => AOptions.IdleTimeout = TimeSpan.FromMinutes(Constants.Sessions.IdleTimeout));
            AServices.AddAntiforgery(AOption =>
            {
                AOption.Cookie.Name = "AntiForgeryTokenCookie";
                AOption.HeaderName = "AntiForgeryTokenField";
            });

            AServices.AddSingleton<IAppLogger, AppLogger.AppLogger>();
            AServices.AddDbContext<MainDbContext>(AOptions => AOptions.UseSqlServer(FConfiguration.GetConnectionString("DbConnect"), AAddOptions => AAddOptions.EnableRetryOnFailure()));

            AServices.AddScoped<ILogicContext, LogicContext>();

            AServices.AddResponseCompression(AOptions =>
            {
                AOptions.Providers.Add<GzipCompressionProvider>();
            });

        }

        public void Configure(IApplicationBuilder AApplication, IWebHostEnvironment AEnvironment)
        {

            AApplication.UseResponseCompression();

            if (AEnvironment.IsDevelopment())
            {
                AApplication.UseDeveloperExceptionPage();
            }
            else
            {
                AApplication.UseExceptionHandler("/Error");
            }

            AApplication.UseStaticFiles();
            AApplication.UseRouting();
            AApplication.UseAuthorization();
            AApplication.UseBrowserLink();
            AApplication.UseSession();
            AApplication.UseEndpoints(AEndpoints =>
            {
                AEndpoints.MapRazorPages();
                AEndpoints.MapControllers();
            });

        }

    }

}
