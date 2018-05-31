using InfomationManager.Grains;
using InfomationManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace InfomationManager.SiloHost
{
    class Program
    {
        /// <summary>
        /// Microsoft SQL Server invariant name string.
        /// </summary>
        public const string InvariantNameSqlServer = "System.Data.SqlClient";

        /// <summary>
        /// Oracle Database server invariant name string.
        /// </summary>
        public const string InvariantNameOracleDatabase = "Oracle.DataAccess.Client";

        /// <summary>
        /// SQLite invariant name string.
        /// </summary>
        public const string InvariantNameSqlLite = "System.Data.SQLite";

        /// <summary>
        /// MySql invariant name string.
        /// </summary>
        public const string InvariantNameMySql = "MySql.Data.MySqlClient";

        /// <summary>
        /// PostgreSQL invariant name string.
        /// </summary>
        public const string InvariantNamePostgreSql = "Npgsql";

        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=OrleansLearn;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true";
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "UserTestApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(SystemUserGrain).Assembly).WithReferences())
                .ConfigureServices(services => services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(connectionString)))
                .ConfigureLogging(logging => logging.AddConsole())
                .AddAdoNetGrainStorage("LocalTestStore", opt =>
                 {
                     opt.ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=OrleansUserTest;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true";
                     opt.Invariant = InvariantNameSqlServer;
                     opt.UseJsonFormat = true;
                 });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
