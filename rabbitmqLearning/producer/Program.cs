using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using RabbitMQ.Client;
using rabbitmqLearning.producer;

namespace rabbitmqLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int channelCount = 10;
            int maxCount = 100;
            string clientName = "TestClient";
            IConnection connection = null;
            List<Sender> senders = new List<Sender>(channelCount);
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var factory = new ConnectionFactory();
                factory.HostName = "10.12.32.91";
                connection = factory.CreateConnection(clientName);

                for (int i = 0; i < channelCount; i++)
                {
                    //var queueName = $"{clientName}{i + 1}";
                    var queueName = clientName;
                    senders.Add(new Sender(connection, $"Sender-{clientName}{i + 1}", queueName, cancellationTokenSource.Token));
                }
                senders.ForEach(i => Task.Run(() => i.DoWork(maxCount)));
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
