using Discount_gRPC.Data;
using Discount_gRPC.Data.Interfaces;
using Discount_gRPC.Helpers;
using Discount_gRPC.Repositories;
using Discount_gRPC.Repositories.Interfaces;
using Discount_gRPC.Services;
using Discount_gRPC.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Discount_gRPC
{
    public class Startup
    {
        //adding iconfiguration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            #region Configuration Dependencies
            services.Configure<PromoCodeDBSettings>(Configuration.GetSection(nameof(PromoCodeDBSettings)));
            services.AddSingleton<IPromoCodeDBSettings>(sp =>
                sp.GetRequiredService<IOptions<PromoCodeDBSettings>>().Value);
            #endregion

            #region Project Dependencies
            services.AddTransient<IDiscountContext, DiscountContext>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddAutoMapper(x => x.AddProfile(new AutoMapperProfiles()));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<PromoCodeService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
