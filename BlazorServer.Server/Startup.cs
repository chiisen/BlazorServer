using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace BlazorServer.Server
{

    public class Startup
    {
        public static IConfiguration JsonConfig { get; set; }

        public Startup(IConfiguration configuration)
        {
            // 讀取客制化 Json 檔案
            // Json 檔案格為 appsettings.[目前組態].json
            string appsettingJson_ = $"appsettings.{DebuggingProperties.Config}.json";

            // 讀取目錄內客制化的 Json 檔案
            JsonConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appsettingJson_, optional: true)
            .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();


            #region Register the MiniProfiler services

            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");

            #endregion Register the MiniProfiler services


            #region Register the Swagger services

            // Register the Swagger services
            services.AddSwaggerDocument();

            #endregion Register the Swagger services


            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            #region Register the MiniProfiler

            app.UseMiniProfiler();

            #endregion Register the MiniProfiler


            #region Register the Swagger generator and the Swagger UI middlewares

            // Register the Swagger generator and the Swagger UI middlewares

            app.UseSwagger();
            app.UseSwaggerUi3();

            #endregion Register the Swagger generator and the Swagger UI middlewares


            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
        }
    }
}
