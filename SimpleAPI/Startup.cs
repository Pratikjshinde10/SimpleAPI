using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(options =>
                {
                    Configuration.Bind("AzureAd", options);
                    options.Events = new JwtBearerEvents();
                    options.Events.OnTokenValidated = async context =>
                    {
                        string[] allowedClientApps =
                        {
                            "76f5b655-0364-427c-a730-6e6e8f3211e7"
                        };
                        string clientappId = context?.Principal?.Claims
                            .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                        if (!allowedClientApps.Contains(clientappId))
                        {
                            throw new UnauthorizedAccessException("The client app is not permitted to access this API");
                        }

                        await Task.CompletedTask;
                    };

                }, options =>
                {
                    Configuration.Bind("AzureAd", options);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleAPI", Version = "v1" });
            });
            services.AddControllers();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>  {
                                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleAPI v1");
                                     c.RoutePrefix = string.Empty;
                                     c.OAuthClientId("76f5b655-0364-427c-a730-6e6e8f3211e7");
                                     c.OAuthUsePkce();
                                     c.OAuthScopeSeparator(" ");
                                   }
                            );
            
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
