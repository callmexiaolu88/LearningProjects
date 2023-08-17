using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Storage;

namespace Grain.Storage
{
    public static class CustomStorageServiceExtension
    {
        public static ISiloHostBuilder AddCustomStorageProvider(this ISiloHostBuilder builder, string providerName, Action<CustomStorageOptions> options)
        {
           return  builder.ConfigureServices(service =>
            {
                _ = service.AddOptions<CustomStorageOptions>(providerName).Configure(options);
                _ = service.AddSingletonNamedService<IGrainStorage>(providerName, (serviceProvider, providerName) =>
                  {
                      var optionsSnapshot = serviceProvider.GetRequiredService<IOptionsSnapshot<CustomStorageOptions>>();
                      var options = optionsSnapshot.Get(providerName);
                      var clusterOptions = serviceProvider.GetRequiredService<IOptions<ClusterOptions>>();
                      return ActivatorUtilities.CreateInstance<CustomStorage>(serviceProvider, providerName, options, clusterOptions.Value);
                  });
                _ = service.AddSingletonNamedService(providerName, (serviceProvider, providerName) =>
                  {
                      var customStorage = serviceProvider.GetRequiredServiceByName<IGrainStorage>(providerName);
                      return (ILifecycleParticipant<ISiloLifecycle>)customStorage;
                  });
            });
        }
    }
}
