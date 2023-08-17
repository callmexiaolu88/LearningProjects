using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitmqLearning.consumer
{
    public class Receiver
    {
        public string Name { get; init; }
        public string QueueName { get; init; }
        readonly IConnection _connection;
        readonly CancellationToken _cancellationToken;

        public Receiver(IConnection connection, string name, string queueName, CancellationToken cancellationToken)
        {
            _connection = connection;
            Name = name;
            QueueName = queueName;
            _cancellationToken = cancellationToken;
        }

        public void DoWork(int maxSleep)
        {
            var random = new Random(DateTime.Now.Millisecond);

            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare("test-fanout", ExchangeType.Fanout);
            channel.QueueDeclare(QueueName);
            channel.QueueBind(QueueName, "test-fanout", string.Empty, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                System.Console.WriteLine($" [{Name}] received message: {message} .");
                var count = random.Next(1, maxSleep);
                System.Console.WriteLine($" [{Name}] sleep {count}s .");
                Thread.Sleep(count * 1000);
                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicQos(0, 1, false);
            channel.BasicConsume(queue: QueueName, consumer: consumer);
            _cancellationToken.WaitHandle.WaitOne();
            Console.WriteLine($" [{Name}] exsit.");
        }
    }
}