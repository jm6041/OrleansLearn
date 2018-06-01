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
            IClusterClient client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "UserTestApp";
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ISystemUserGrain).Assembly).WithReferences())
                .Build();
            services.AddSingleton(client);
            return services;
        }
    }
}
