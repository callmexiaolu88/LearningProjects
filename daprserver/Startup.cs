using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace daprserver
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
            services.AddActors(option =>
            {
                option.Actors.RegisterActor<TestActor>();
                option.HttpEndpoint = "http://10.12.32.91:3004";
            });
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
                endpoints.MapActorsHandlers();
                endpoints.MapGet("test", async context =>
                {
                    var arr = JsonSerializer.SerializeToUtf8Bytes($"test server:{context.Request.Host}");
                    await context.Response.Body.WriteAsync(arr);
                });
                endpoints.MapPost("testsub", async context =>
                {
                    var result = await JsonSerializer.DeserializeAsync<object>(context.Request.BodyReader.AsStream());
                    context.RequestServices.GetService<ILogger<Startup>>().LogInformation(JsonSerializer.Serialize(result));
                    var arr = JsonSerializer.SerializeToUtf8Bytes(new
                    {
                        status = "SUCCESS"
                    });
                    await context.Response.Body.WriteAsync(arr);
                });
            });
        }
    }
}
