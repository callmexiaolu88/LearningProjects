using System.Net.Http;
using System;
using Dapr.Client;
using System.Text.Json;
using System.Collections.Generic;
using Dapr.Actors.Client;
using Dapr.Actors;

namespace daprclient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new DaprClientBuilder().UseGrpcEndpoint("http://10.12.32.91:50001").UseHttpEndpoint("http://10.12.32.91:3500").Build();
            setState(client);
            getSecretState(client);
            getState(client);
            invokeService(client);
            publishMessage(client);
            outBinding(client);
            getCleanState(client, "bindingkey");
            invokeActors();
            Console.WriteLine("Done!");
        }

        private static void setState(DaprClient client)
        {
            Console.WriteLine("setState()");
            client.SaveStateAsync<string>("statestore", "TestBinding", "123").GetAwaiter().GetResult();
            var entity = client.GetStateEntryAsync<string>("statestore", "TestBinding").Result;
            Console.WriteLine(entity.Value);
            Console.WriteLine("setState() done");
        }

        private static void getState(DaprClient client)
        {
            Console.WriteLine("getState()");
            var entity = client.GetStateEntryAsync<string>("statestore", "test").Result;
            Console.WriteLine(entity.Value);
            Console.WriteLine("getState() done");
        }

        private static void getCleanState(DaprClient client, string name)
        {
            Console.WriteLine("getCleanState()");
            var entity = client.GetStateEntryAsync<string>("cleanstatestore", name).Result;
            Console.WriteLine(entity.Value);
            Console.WriteLine("getCleanState() done");
        }

        private static void getSecretState(DaprClient client)
        {
            Console.WriteLine("getSecretState()");
            var result = client.GetSecretAsync("secretstore", "redispwd").GetAwaiter().GetResult();
            Console.WriteLine(JsonSerializer.Serialize(result));
            Console.WriteLine("getSecretState() done");
        }

        private static void invokeService(DaprClient client)
        {
            Console.WriteLine("invokeService()");
            for (int i = 0; i < 10; i++)
            {
                var req = client.InvokeMethodAsync<string>(HttpMethod.Get, "server", "test").Result;
                Console.WriteLine(req);
            }
            Console.WriteLine("invokeService() done");
        }

        private static void publishMessage(DaprClient client)
        {
            Console.WriteLine("publishMessage()");
            client.PublishEventAsync<string>("pubsub", "testmethod", $"this is publish:[{Guid.NewGuid()}]").Wait();
            Console.WriteLine("publishMessage() done");
        }

        private static void outBinding(DaprClient client)
        {
            Console.WriteLine("outBinding()");
            client.InvokeBindingAsync("redisoutbinding", "create", "TestBinding123", new Dictionary<string, string>{
                {"key","bindingkey"}
            }).Wait();
            Console.WriteLine("outBinding() done");
        }

        private static void invokeActors()
        {
            Console.WriteLine("invokeActors()");
            var proxy = ActorProxy.Create(ActorId.CreateRandom(), "TestActor", new ActorProxyOptions { HttpEndpoint = "http://10.12.32.91:3500" });
            var result= proxy.InvokeMethodAsync<string>("GetIdAsync").Result;
            Console.WriteLine($"TestActor.Get: [{result}]");
            Console.WriteLine("invokeActors() done");
        }
    }
}
