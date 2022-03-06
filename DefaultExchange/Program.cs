using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IConnection con = factory.CreateConnection();
IModel channel = con.CreateModel();

channel.QueueDeclare(
    "my.queue1",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.queue2",
    true,
    false,
    false,
    null);

channel.BasicPublish(
    "",
    "my.queue1",
    null,
    Encoding.UTF8.GetBytes("Message Whith Routing Key my.queue1"));

channel.BasicPublish(
    "",
    "my.queue2",
    null,
    Encoding.UTF8.GetBytes("Message Whith Routing Key my.queue2"));