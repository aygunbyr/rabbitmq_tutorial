using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "mainexchange", 
    type: ExchangeType.Direct);

channel.ExchangeDeclare(
    exchange: "dlx", 
    type: ExchangeType.Fanout);

channel.QueueDeclare(
    queue: "mainexchangequeue",
    arguments: new Dictionary<String, Object>{
        {"x-dead-letter-exchange", "dlx"},
        {"x-message-ttl", 1000}
    });
channel.QueueBind("mainexchangequeue", "mainexchange", "test");

var mainConsumer = new EventingBasicConsumer(channel);
mainConsumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Main - Received new message: {message}");
};
//channel.BasicConsume(queue: "mainexchangequeue", autoAck: true, consumer: mainConsumer);

channel.QueueDeclare(queue: "dlxexchangequeue");
channel.QueueBind("dlxexchangequeue", "dlx", "");

var dlxConsumer = new EventingBasicConsumer(channel);

dlxConsumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"DLX - Received new message: {message}");
};
channel.BasicConsume(queue: "dlxexchangequeue", autoAck: true, consumer: dlxConsumer);

Console.WriteLine("Consuming");

Console.ReadKey();

