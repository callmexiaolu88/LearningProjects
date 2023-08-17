using System;
using System.Threading.Tasks;
using Grain.Implement;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans;
using Orleans.Persistence;
using Orleans.Statistics;
using Grain.Infrastructures.Filters;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Grain.Storage;
using Orleans.Runtime;

namespace Grain.Silo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Orleans Server!");
            await RunMainAsync();
        }

        static async Task<int> RunMainAsync()
        {
            try
            {
                using var siloHost = await BuildSiloHost();
                Console.WriteLine("Starting silo host");
                await siloHost.StartAsync();

                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();
                await siloHost.StopAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

        private static async Task<ISiloHost> BuildSiloHost()
        {
            var random = new Random(DateTime.Now.Second);
            var invariant = "System.Data.SqlClient";
            var connectionString = @"Data Source=(localdb)\.\orleans;Initial Catalog=orleans;Integrated Security=True";
            var siloHost = new SiloHostBuilder()
                .UseAdoNetClustering(opt =>
                {
                    opt.ConnectionString = connectionString;
                    opt.Invariant = invariant;
                })
                .ConfigureEndpoints(random.Next(11111, 22222), random.Next(11111, 22222))
                .AddAdoNetGrainStorage("testStorageProvidor", opt =>
                {
                    opt.ConnectionString = connectionString;
                    opt.Invariant = invariant;
                    opt.UseJsonFormat = true;
                })
                .AddSimpleMessageStreamProvider("customStreamProvider")
                .ConfigureLogging(logging => logging.AddConsole())
                .Configure<ClusterOptions>(opt =>
                {
                    opt.ClusterId = "testDev";
                    opt.ServiceId = "testService";
                })
                .AddCustomStorageProvider("customStorageProvider", opt => opt.RootDirectory = @"E:\orleansCustomStorage")
                //.AddIncomingGrainCallFilter<CaptureCallingFilter>()
                //.UsePerfCounterEnvironmentStatistics()
                //.UseDashboard()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
                .Build();

            return await Task.FromResult(siloHost);
        }
    }
}
