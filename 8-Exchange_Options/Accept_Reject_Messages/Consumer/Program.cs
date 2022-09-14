using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "acceptrejectexchange", 
    type: ExchangeType.Fanout);

channel.QueueDeclare(
    queue: "letterbox");
channel.QueueBind("letterbox", "acceptrejectexchange", "test");

var mainConsumer = new EventingBasicConsumer(channel);

mainConsumer.Received += (model, ea) => 
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

    Console.WriteLine($"Main - Received new message: {message}");
};
channel.BasicConsume(queue: "letterbox", consumer: mainConsumer);

Console.WriteLine("Consuming");

Console.ReadKey();

