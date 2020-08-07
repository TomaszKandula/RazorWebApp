using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using SecureWebApp.Helpers;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.DnsLookup;
using SecureWebApp.Extensions.AppLogger;

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

            AServices.AddMvc(Option => Option.CacheProfiles
                .Add("ResponseCache", new CacheProfile()
                {
                    Duration = 5,
                    Location = ResponseCacheLocation.Any,
                    NoStore = false
                }));

            AServices.AddMvc(Option => Option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            AServices.AddRazorPages().AddRazorRuntimeCompilation();
            AServices.AddControllers();

            AServices.AddSession(Options => Options.IdleTimeout = TimeSpan.FromMinutes(Constants.Sessions.IdleTimeout));
            AServices.AddAntiforgery(Option => Option.HeaderName = "X-CSRF-TOKEN");

            AServices.AddSingleton<IDnsLookup, DnsLookup>();
            AServices.AddSingleton<IAppLogger, AppLogger>();
            AServices.AddDbContext<MainDbContext>(Options => Options.UseSqlServer(FConfiguration.GetConnectionString("DbConnect"), AddOptions => AddOptions.EnableRetryOnFailure())); 

            AServices.AddResponseCompression(Options =>
            {
                Options.Providers.Add<GzipCompressionProvider>();
            });

        }

        public void Configure(IApplicationBuilder AApplication, IWebHostEnvironment AEnvironment)
        {

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
            AApplication.UseEndpoints(Endpoints =>
            {
                Endpoints.MapRazorPages();
                Endpoints.MapControllers();
            });

        }

    }

}
