using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShoppingWeb.ApiCollection.APIs;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.ApiCollection.Settings;
using ShoppingWeb.Discount_gRPC_Services;
using ShoppingWeb.Helpers;
using System;

namespace ShoppingWeb
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
            #region HttpClient and Api settings
            services.Configure<ApiSettings>(Configuration.GetSection(nameof(ApiSettings)));
            services.AddHttpClient();
            services.AddSingleton<IApiSettings>(s => s.GetRequiredService<IOptions<ApiSettings>>().Value);
            #endregion

            #region AutoMapper settings
            services.AddAutoMapper(x => x.AddProfile(new AutoMapperProfiles()));
            #endregion

            #region Discount gRPC settings
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddGrpcClient<PromoDiscountService.PromoDiscountServiceClient>
                        (o => { o.Address = new Uri(Configuration["gRPC_Settings:Discount_gRPC_URL"]);
                            o.ChannelOptionsActions.Add(channelOptions => channelOptions.Credentials = ChannelCredentials.Insecure);
                        });
            services.AddScoped<DiscountPromogRPCService>();
            #endregion

            #region API interfaces
            services.AddTransient<ICatalogApi, CatalogApi>();
            services.AddTransient<IBasketApi, BasketApi>();
            services.AddTransient<IOrderingApi, OrderingApi>();
            #endregion

            services.AddRazorPages();
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
            }

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
