using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

var Factory = new ConnectionFactory{HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox", 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments:null);

var random = new Random();

int messageID = 1;

while(true) {
    var publishingTime = random.Next(1, 4);

    var message = $"Sending message #{messageID}";

    var encodedMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "letterbox", null, encodedMessage);

    Console.WriteLine($"Send message: {message}");

    Task.Delay(TimeSpan.FromSeconds(publishingTime)).Wait();

    messageID++;
}


