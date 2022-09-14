using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "simplehashing", type: "x-consistent-hash");

var message = "hello hash the routing and pass me on please";

var body = Encoding.UTF8.GetBytes(message);

var routingKey = "Hash me!1234567890"; // By the routing key change, queue may be changed

channel.BasicPublish("simplehashing", routingKey, null, body);

Console.WriteLine($"Published message: {message}");
