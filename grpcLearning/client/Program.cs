using System.Net;
using System.Net.Security;
using System;
using System.Text.Json;
using Grpc.Net.Client;
using System.Net.Http;
using Grpc.Core.Interceptors;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            Console.WriteLine("Hello World!");
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //var invoker = channel.Intercept(new BlockingUnaryCall());
            var client = new TestGrpc.Imples.TestService.TestServiceClient(channel);
            var result = client.GetService(new TestGrpc.Imples.GetRequest { Name = "12" });
            System.Console.WriteLine(JsonSerializer.Serialize(result));
        }
    }
}
