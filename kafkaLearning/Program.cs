using System.Threading;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace kafkaLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Task.Run(() => Producer());
                Task.Run(() => Consumer());
                Console.Read();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static async Task Producer()
        {
            IProducer<Null, string> producer = null;
            try
            {
                Console.WriteLine("Hello, this is producer!");
                var config = new ProducerConfig { BootstrapServers = "10.12.32.91:9094" };
                producer = new ProducerBuilder<Null, string>(config).Build();
                var count = 1;
                while (true)
                {
                    var deliveryResult = await producer.ProduceAsync("testTopic", new Message<Null, string> { Value = $"Test {count++:x16}" });
                    Print(ConsoleColor.Green, $"[Producer]: {JsonSerializer.Serialize(deliveryResult)}");
                    Thread.Sleep(1000);
                }

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                producer?.Dispose();
            }
        }

        static void Consumer()
        {
            IConsumer<Null, string> consumer = null;
            try
            {
                Console.WriteLine("Hello, this is consumer!");
                var config = new ConsumerConfig
                {
                    BootstrapServers = "10.12.32.91:9094",
                    GroupId = "t1",
                    
                };
                consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe("testTopic");
                while (true)
                {
                    var result = consumer.Consume(CancellationToken.None);
                    Print(ConsoleColor.Yellow, $"[Consumer]: {JsonSerializer.Serialize(result)}");
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                consumer?.Dispose();
            }
        }
        static AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        static void Print(ConsoleColor color, string message)
        {
            autoResetEvent.WaitOne();

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();

            autoResetEvent.Set();
        }
    }
}
