using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "headersexchange", type: ExchangeType.Headers);

var message = "This message will be sent with headers";

var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<String, Object>{
     {"name", "brian"}
};

channel.BasicPublish("headersexchange", "", properties, body);

Console.WriteLine($"Published message: {message}");
