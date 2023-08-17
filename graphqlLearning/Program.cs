using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace graphqlLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            T1();
            CreateHostBuilder(args).Build().Run();
        }

        public static void T1([CallerMemberName] string callerName = ""){
            T2();
            System.Console.WriteLine($"T1 caller:{callerName}");
        }

        public static void T2([CallerMemberName] string callerName = ""){
            System.Console.WriteLine($"T2 caller:{callerName}");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
