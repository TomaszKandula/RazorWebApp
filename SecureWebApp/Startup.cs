using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecureWebApp.Extensions.ConnectionService;
using SecureWebApp.Models.Database;

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

            // Add MVC
            AServices.AddMvc(Option => Option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            // Add runtime compilation
            AServices.AddRazorPages().AddRazorRuntimeCompilation();

            // Register (a'priori) connection service holding connection string(s) to database(s)
            AServices.AddScoped<IConnectionService, ConnectionService>();

            // Operational database
            AServices.AddDbContext<MainDbContext>();

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
            AApplication.UseEndpoints(Endpoints =>
            {
                Endpoints.MapRazorPages();
            });

        }

    }

}
