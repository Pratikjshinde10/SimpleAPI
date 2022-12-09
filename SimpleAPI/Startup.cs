using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace SimpleAPI
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
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(
                        Configuration["https://graph.microsoft.com/beta/me/"],
                        Configuration.GetValue<string>("Directory.Read.All")
            ).AddInMemoryTokenCaches();
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme, options => {
                   options.TokenValidationParameters.ValidIssuers = new[] { "https://sts.windows.net/fbadfc59-b650-4877-b930-13cbaaf2e0dc/" };
                   options.TokenValidationParameters.ValidAudiences = new[] { "0c81e5bc-a5ec-4b13-9c78-2fcbb75b1ba0/.default" };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleAPI v1"));
            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
