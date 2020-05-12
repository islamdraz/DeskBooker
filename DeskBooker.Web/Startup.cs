using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Processor;
using DeskBooker.Persistance;
using DeskBooker.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeskBooker.Web
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
            services.AddRazorPages();
            //var connectionString = "DataSource:Memory";
            //var connection = new SqliteConnection(connectionString);
            //connection.Open();

            //services.AddDbContext<DeskBookerContext>(builder =>
            //{
            //    builder.useIn
               
            //});
            services.AddDbContext<DeskBookerContext>(
                options => options.UseInMemoryDatabase(databaseName: "deskBookingDb")
                , ServiceLifetime.Scoped
                , ServiceLifetime.Scoped);

            services.AddScoped<IDeskBookingRepository, DeskBookingRepository>();
            services.AddScoped<IDeskRepository, DeskRepository>();
            services.AddScoped<IDeskBookingRequestProcessor, DeskBookingRequestProcessor>();

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
