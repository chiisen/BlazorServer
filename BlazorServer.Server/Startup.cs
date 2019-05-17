using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Swagger;

namespace BlazorServer.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            #region ���U Swagger
            // ���U Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    // name: ���� SwaggerDocument �� URL ��m�C
                    name: "v1",
                    // info: �O�Ω� SwaggerDocument ������T�����(���e�D����)�C
                    info: new Info
                    {
                        Title = "Web API",
                        Version = "�������G1.0.0",
                        Description = "�o�O ASP.NET Core 2.1 API",
                        TermsOfService = "�L",
                        Contact = new Contact
                        {
                            Name = "John Wu",
                            Url = "https://blog.johnwu.cc"
                        },
                        License = new License
                        {
                            Name = "CC BY-NC-SA 4.0",
                            Url = "https://creativecommons.org/licenses/by-nc-sa/4.0/"
                        }
                    }
                );
                // �� Swagger JSON and UI�]�mxml���ɵ������|
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//������ε{�ǩҦb�ؿ��]����A�����u�@�ؿ��v�T�A��ĳ�ĥΦ���k������|�^
                var xmlPath = Path.Combine(basePath, "Swagger.xml");
                c.IncludeXmlComments(xmlPath);
            });
            #endregion ���U Swagger

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

            #region ���U Swagger
            // ���U Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    // url: �ݰt�X SwaggerDoc �� name�C "/swagger/{SwaggerDoc name}/swagger.json"
                    url: "/swagger/v1/swagger.json",
                    // name: �Ω� Swagger UI �k�W����ܤ��P������ SwaggerDocument ��ܦW�٨ϥΡC
                    name: "RESTful API v1.0.0"
                );
            });
            #endregion ���U Swagger

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
        }
    }
}
