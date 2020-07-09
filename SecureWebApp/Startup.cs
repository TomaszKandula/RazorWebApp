using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using SecureWebApp.Middleware;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.DnsLookup;
using SecureWebApp.Extensions.AppLogger;
using SecureWebApp.Extensions.DataAccess;
using SecureWebApp.Extensions.ConnectionService;

namespace SecureWebApp
{

    public class Startup
    {

        public IConfiguration FConfiguration { get; }

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
            AServices.AddSession(Options => Options.IdleTimeout = TimeSpan.FromMinutes(5));

            AServices.AddSingleton<IDnsLookup, DnsLookup>();
            AServices.AddSingleton<IAppLogger, AppLogger>();
            AServices.AddScoped<IConnectionService, ConnectionService>();
            AServices.AddTransient<IDataAccess, DataAccess>();
            AServices.AddDbContext<MainDbContext>();

            AServices.AddResponseCompression(Options =>
            {
                Options.Providers.Add<GzipCompressionProvider>();
            });

        }

        public void Configure(IApplicationBuilder AApplication, IWebHostEnvironment AEnvironment)
        {

            AApplication.UseMiddleware<Authorization>();

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
