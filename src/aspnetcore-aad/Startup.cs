using aspnetcore_aad.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Threading.Tasks;

namespace aspnetcore_aad
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private async Task<string> GetConnectionStringFromKV()
        {
            // Get the credential of user assigned identity
            var credential = new DefaultAzureCredential();
            // Get secret from key vault
            var kvClient = new SecretClient(new Uri(Configuration["KeyVaultUri"]), credential);
            var secretBundle = await kvClient.GetSecretAsync("aksdemo-connstr");

            return secretBundle.Value.Value;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var connectionString = GetConnectionStringFromKV().GetAwaiter().GetResult();

                services.AddDbContext<TodoContext>(
                    options => options.UseSqlServer(connectionString)
                );
            }
            catch
            {
                services.AddDbContext<TodoContext>(
                    options => options.UseInMemoryDatabase("tododb"));
            }

            services.AddMicrosoftIdentityWebAppAuthentication(Configuration);

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();
            // Forward headers
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Log debug info for the forwarded headers
            app.UseForwardedHeaders();
            app.Use(async (context, next) =>
            {
                // Request method, scheme, and path
                logger.LogInformation("Request Method: {Method}", context.Request.Method);
                logger.LogInformation("Request Scheme: {Scheme}", context.Request.Scheme);
                logger.LogInformation("Request Path: {Path}", context.Request.Path);

                // Headers
                foreach (var header in context.Request.Headers)
                {
                    logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
                }

                // Connection: RemoteIp
                logger.LogInformation("Request RemoteIp: {RemoteIpAddress}",
                    context.Connection.RemoteIpAddress);

                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
