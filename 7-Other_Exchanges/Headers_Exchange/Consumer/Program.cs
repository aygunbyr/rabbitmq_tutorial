using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "headersexchange", type: ExchangeType.Headers);

channel.QueueDeclare(queue: "letterbox");

var bindingArguments = new Dictionary<String, Object>{
    {"x-match", "any"},
    {"name", "brian"},
    {"age", "21"}
};

channel.QueueBind("letterbox", "headersexchange", "", bindingArguments);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message received: {message}");
};

channel.BasicConsume(
    queue: "letterbox", 
    autoAck: true, 
    consumer: consumer);

Console.WriteLine("Consuming");

Console.ReadKey();



