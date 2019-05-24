using System.Security.Cryptography.X509Certificates;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IdentityServer
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        
        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var configFileName = Path.Combine(Directory.GetCurrentDirectory(), "serversettings.json");

            if (!File.Exists(configFileName))
            {
                throw new FileNotFoundException($"Settings file is missing! {configFileName}");
            }

            var settings = JsonConvert.DeserializeObject<ServerSettings>(File.ReadAllText(configFileName));
            
            services.AddAuthentication().AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = settings.gcid;
                    options.ClientSecret = settings.gcs;
                });

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients());

            services.AddCors(options =>
            {
                options.AddPolicy("sip.mgr",
                bldr =>
                {
                    bldr.WithOrigins(
                        "https://localhost:4200",
                        "ms-appx-web://microsoft.microsoftedge",
                        "https://baalansellers.github.io"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var fileName = Path.Combine($"{Environment.WebRootPath}\\assets", "identity.pfx");
                
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException($"Signing Certificate is missing! {fileName}");
                }

                var identityCert = new X509Certificate2(
                    fileName,
                    settings.cp,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.Exportable
                );

                builder.AddSigningCredential(identityCert);
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseCors("sip.mgr");

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }
    }
}