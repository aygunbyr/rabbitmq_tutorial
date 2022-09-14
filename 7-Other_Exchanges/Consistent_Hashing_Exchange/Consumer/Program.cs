using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "simplehashing", type: "x-consistent-hash");

channel.QueueDeclare(queue: "letterbox1");
channel.QueueDeclare(queue: "letterbox2");

channel.QueueBind("letterbox1", "simplehashing", "1"); // 25% of hash space
channel.QueueBind("letterbox2", "simplehashing", "3"); // 75% of hash space

var consumer1 = new EventingBasicConsumer(channel);

consumer1.Received += (model, ea) => {
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Queue1 received new message: {message}");
};

channel.BasicConsume(queue: "letterbox1", autoAck: true, consumer: consumer1);

var consumer2 = new EventingBasicConsumer(channel);

consumer2.Received += (model, ea) => {
    var body = ea.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Queue2 received new message: {message}");
};

channel.BasicConsume(queue: "letterbox2", autoAck: true, consumer: consumer2);

Console.WriteLine("Consuming");

Console.ReadKey();



