using System;
using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox", 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments:null);

var message = "This is my first message";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("", "letterbox", null, body);

Console.WriteLine($"Published message: {message}");
