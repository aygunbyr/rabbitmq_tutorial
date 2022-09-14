using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "acceptrejectexchange", 
    type: ExchangeType.Fanout);

while( true ) {
    var message = "Lets send this";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("acceptrejectexchange", "test", null, body);
    Console.WriteLine($"Published message: {message}");

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
}


