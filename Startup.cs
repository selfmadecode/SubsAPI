using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using SubsAPI.Data;
using SubsAPI.Helpers;
using SubsAPI.Models;
using SubsAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI
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
            ConfigureEntityFrameworkDbContext(services);
            ConfigureSwagger(services);

            ConfigureDIService(services);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subs API v1"));
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void ConfigureEntityFrameworkDbContext(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                          options.UseSqlServer(connectionString));
        }

        private void ConfigureDIService(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ITokenConfigService, TokenConfigService>();
            services.Configure<TokenLenght>(Configuration.GetSection("TokenLenght"));
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                       new OpenApiInfo
                       {
                           Title = "SUBS API",
                           Version = "v1",
                           Description = "Yellowdot Subs Platform",
                           Contact = new OpenApiContact
                           {
                               Name = "Yellowdot",
                               Email = "Info@yellowdot.com"
                           },
                           License = new OpenApiLicense
                           {
                               Name = "MIT License",
                               Url = new Uri("https://en.wikipedia.org/wiki/MIT_Lincense")
                           }
                       });
                c.DescribeAllParametersInCamelCase();
            });
        }
    }
}
