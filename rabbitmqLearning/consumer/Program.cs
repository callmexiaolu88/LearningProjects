using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using RabbitMQ.Client;
using rabbitmqLearning.consumer;

namespace rabbitmqLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int channelCount = 15;
            int maxSleep = 5;//per second
            string clientName = "TestClient";
            IConnection connection = null;
            List<Receiver> reveivers = new List<Receiver>(channelCount);
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var factory = new ConnectionFactory();
                factory.HostName = "10.12.32.91";
                connection = factory.CreateConnection(clientName);

                for (int i = 0; i < channelCount; i++)
                {
                    var queueName = $"{clientName}{i + 1}";
                    //var queueName = clientName;
                    reveivers.Add(new Receiver(connection, $"Receiver-{clientName}{i + 1}", queueName, cancellationTokenSource.Token));
                }
                reveivers.ForEach(i => Task.Run(() => i.DoWork(maxSleep)));
                System.Console.WriteLine(" Press [enter] to exit.");
                Console.Read();
            }
            catch (System.Exception ex)
            {
                UtilitiesHelper.Debug(ex);
            }
            finally
            {
                cancellationTokenSource.Cancel();
                connection?.Dispose();
            }
        }


    }
}
