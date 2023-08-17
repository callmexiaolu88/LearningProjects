using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace rabbitmqLearning.producer
{
    public class Sender
    {
        public string Name { get; init; }
        public string QueueName { get; init; }
        readonly IConnection _connection;
        readonly CancellationToken _cancellationToken;

        public Sender(IConnection connection, string name, string queueName, CancellationToken cancellationToken)
        {
            _connection = connection;
            Name = name;
            QueueName = queueName;
            _cancellationToken = cancellationToken;
        }

        public void DoWork(int maxCount, int sleep = 1000)
        {
            var random = new Random(Thread.CurrentThread.ManagedThreadId);
            var count = random.Next(10, maxCount);
            using var channel = _connection.CreateModel();
            //channel.QueueDeclare(QueueName, exclusive: false, autoDelete: false);
            
            channel.ExchangeDeclare("test-fanout", ExchangeType.Fanout);
            for (int i = 1; i <= count; i++)
            {
                var message = $"[{DateTime.Now:yyyy:mm:dd HH:MM:ss:ff}] hello world, I'm {Name}";
                var messagebytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("test-fanout", "", null, messagebytes);
                Console.WriteLine($" [{i}] [{Name}] sent {message} .");
                count--;
                Thread.Sleep(sleep);
            }
            _cancellationToken.WaitHandle.WaitOne();
            Console.WriteLine($" [{Name}] exsit.");
        }

    }
}