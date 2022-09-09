using System;
using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory{HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "mytopicexchange",
    type: ExchangeType.Topic
);

var userPaymentsMessage = "A european user paid for something";

var userPaymentsBody = Encoding.UTF8.GetBytes(userPaymentsMessage);

channel.BasicPublish("mytopicexchange", "user.europe.payments", null, userPaymentsBody);

Console.WriteLine($"Published message: {userPaymentsMessage}");

var businessOrderMessage = "A european business ordered goods";

var businessOrderBody = Encoding.UTF8.GetBytes(businessOrderMessage);

channel.BasicPublish("mytopicexchange", "business.europe.order", null, businessOrderBody);

Console.WriteLine($"Published message: {businessOrderMessage}");
