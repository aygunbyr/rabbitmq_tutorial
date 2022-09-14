using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "altexchange", type: ExchangeType.Fanout);

channel.ExchangeDeclare(
    exchange: "mainexchange", 
    type: ExchangeType.Direct,
    arguments: new Dictionary<String, Object>{
        {"alternate-exchange", "altexchange"}
    });

channel.QueueDeclare(queue: "altexchangequeue");
channel.QueueBind("altexchangequeue", "altexchange", "");
var altConsumer = new EventingBasicConsumer(channel);

altConsumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"ALT - Received new message: {message}");
};

channel.BasicConsume(queue: "altexchangequeue", autoAck: true, consumer: altConsumer);

channel.QueueDeclare(queue: "mainexchangequeue");
channel.QueueBind("mainexchangequeue", "mainexchange", "test");

var mainConsumer = new EventingBasicConsumer(channel);

mainConsumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"MAIN - Received new message: {message}");
};

channel.BasicConsume(queue: "mainexchangequeue", autoAck: true, consumer: mainConsumer);

Console.WriteLine("Consuming");

Console.ReadKey();



