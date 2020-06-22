using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            // Add MVC
            AServices.AddMvc(Option => Option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            AServices.AddRazorPages();

            // Register (a'priori) connection service holding connection string(s) to database(s)
            AServices.AddScoped<IConnectionService, ConnectionService>();

            // Operational database
            //AServices.AddDbContext<MainDbModel>();

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
            AApplication.UseEndpoints(Endpoints =>
            {
                Endpoints.MapRazorPages();
            });

        }

    }

}
