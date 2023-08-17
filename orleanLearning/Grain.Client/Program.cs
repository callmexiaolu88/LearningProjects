using System;
using System.Threading.Tasks;
using Grain.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Grain.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Orleans Client!");
            await RunMainAsync();
        }

        static async Task<bool> RunMainAsync()
        {
            try
            {
                using var client= await BuildClient();
                return await DoClientWork(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        static async Task<IClusterClient> BuildClient()
        {
            var client = new ClientBuilder()
                .UseAdoNetClustering(opt =>
                {
                    opt.ConnectionString = @"Data Source=(localdb)\.\orleans;Initial Catalog=orleans;Integrated Security=True";
                    opt.Invariant = "System.Data.SqlClient";
                })
                .Configure<ClusterOptions>(opt =>
                {
                    opt.ClusterId = "testDev";
                    opt.ServiceId = "testService";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
            await client.Connect();
            return client;
        }

        static async Task<bool> DoClientWork(IClusterClient client)
        {
            var user = client.GetGrain<IUserGrain>("1111");
            var r = await user.Exist();
            if (!r)
            {
                return await client.GetGrain<IUserGrain>("121212").Exist();
            }
            return false;
        }
    }
}
