using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ThingsToDoProject.Core.Interface;
using ThingsToDoProject.Core.Provider;
using ThingsToDoProject.Core.Translater;

namespace ThingsToDoProject
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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IGetLatitudeLongitude, GetLatitudeLongitude>();
            services.AddSingleton<IGetData, GetInsideAirportData>();
            services.AddSingleton<IGetOutsideData, GetOutsideAirportData>();
            services.AddSingleton<IGetInsideOutside, GetInsideOutsideAirportData>();
            services.AddSingleton<IGetPlaceData, GetDataOfParticularPlace>();
            services.AddSingleton<IGetDistanceTime, GetDistanceTimeOfParticularPlace>();
            services.AddSingleton<IGetSearch, GetSearchData>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddHttpClient("GoogleClient", client =>
            {
                client.BaseAddress = new Uri("https://maps.googleapis.com");
            });

            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //}));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //app.UseCors(builder =>builder.WithOrigins("thingstodoproject-prod.ap-south-1.elasticbeanstalk.com"));
            //options.AddPolicy("AllowAllOrigins",builder =>{
            //    builder.AllowAnyOrigin();
            //});
            app.UseCors(builder =>builder.WithOrigins("thingstodoproject-prod.ap-south-1.elasticbeanstalk.com").AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = new TimeSpan(0, 0, 180);
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
