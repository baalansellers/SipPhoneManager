using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Api.Sip.Manager.Components;

namespace Api.Sip.Manager
{
    public class Startup
    {
        private const string _policyNameAppSipMgr = "sip.mgr";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = "https://id-sip-mgr.azurewebsites.net";
                        //options.RequireHttpsMetadata = true;
                        options.Audience = "api.sip.manager";
                    });

            services.AddCors(options =>
            {
                options.AddPolicy(_policyNameAppSipMgr,
                bldr =>
                {
                    bldr.WithOrigins(
                        "https://localhost:4200",
                        "https://baalansellers.github.io"
                    )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHttpClient<GitHubService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_policyNameAppSipMgr);

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
