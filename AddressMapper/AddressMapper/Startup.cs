using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager;
using CSVProcessor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AddressMapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<ICsvLoader, CsvLoader>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddOptions();
            services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            var builder = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
         .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var data = ConfigurationRoot.GetSection("Logging").Value;

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
