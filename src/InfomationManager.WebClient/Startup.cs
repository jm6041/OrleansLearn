using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfomationManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace InfomationManager.WebClient
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 获得连接名
            string connectionName = Configuration.GetSection("CurrentConnectionName")?.Value ?? "DefaultConnection";
            // 获得连接字符串
            string connectionString = Configuration.GetConnectionString(connectionName);
            string assemblyFullName = this.GetType().Assembly.FullName;
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyFullName)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddClusterClientDefault();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            IClusterClient clusterClient = app.ApplicationServices.GetService<IClusterClient>();
            appLifetime.ApplicationStarted.Register(OnStarted, clusterClient, false);
            appLifetime.ApplicationStopping.Register(OnStopping, clusterClient, false);
            appLifetime.ApplicationStopped.Register(OnStopped);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async void OnStarted(object state)
        {
            IClusterClient clusterClient = state as IClusterClient;
            try
            {
                await clusterClient.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //for (var i = 0; i < 5; i++)
            //{
            //    try
            //    {
            //        await clusterClient.Connect();
            //        return;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //    }
            //    await Task.Delay(TimeSpan.FromSeconds(5));
            //}
        }

        private void OnStopping(object state)
        {
            IClusterClient clusterClient = state as IClusterClient;
            clusterClient?.Dispose();
        }

        private void OnStopped()
        {
            Task.WaitAll();
        }
    }
}
