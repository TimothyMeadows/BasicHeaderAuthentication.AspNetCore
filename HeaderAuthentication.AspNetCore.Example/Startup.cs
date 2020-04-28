using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeaderAuthentication.AspNetCore.Example.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HeaderAuthentication.AspNetCore.Example
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

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = HeaderAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = HeaderAuthenticationOptions.DefaultScheme;
                })
                .AddHeaderAuthentication(options =>
                {
                    options.HeaderKey = "X-AuthKey";
                    options.Authentication = new HeaderAuthenticate();
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(HeaderAuthenticationOptions.DefaultScheme).RequireAuthenticatedUser().Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
