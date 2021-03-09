using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Promocode.API.Data;
using Promocode.API.Data.Interfaces;
using Promocode.API.Repositories;
using Promocode.API.Repositories.Interfaces;
using Promocode.API.Settings;

namespace Promocode.API
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
            services.AddControllers();

            #region Configuration Dependencies
            services.Configure<PromocodeDBSettings>(Configuration.GetSection(nameof(PromocodeDBSettings)));
            services.AddSingleton<IPromocodeDBSettings>(sp =>
                sp.GetRequiredService<IOptions<PromocodeDBSettings>>().Value);
            #endregion

            #region Project Dependencies
            services.AddTransient<IPromocodeContext, PromocodeContext>();
            services.AddTransient<IPromocodeRepository, PromocodeRepository>();
            #endregion

            #region Swagger Dependencies
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Promocode API", Version = "v1" });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promocode API V1");
            });
        }
    }
}
