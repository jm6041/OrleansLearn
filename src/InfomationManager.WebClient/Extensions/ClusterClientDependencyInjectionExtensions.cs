using InfomationManager.Abstractions;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ClusterClientDependencyInjectionExtensions
    {
        public static IServiceCollection AddClusterClientDefault(this IServiceCollection services)
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=OrleansUserClient;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true";
            IClusterClient client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "UserTestApp";
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ISystemUserGrain).Assembly).WithReferences())
                //.UseAdoNetClustering(opt =>
                //{
                //    opt.ConnectionString = connectionString;
                //    opt.Invariant = "System.Data.SqlClient";
                //})
                .Build();
            services.AddSingleton(client);
            return services;
        }
    }
}
