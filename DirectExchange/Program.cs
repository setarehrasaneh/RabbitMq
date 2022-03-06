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


channel.ExchangeDeclare(
    "ex.direct",
    ExchangeType.Direct,
    true,
    false,
    null);

channel.QueueDeclare(
    "my.infos",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.warnings",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.errors",
    true,
    false,
    false,
    null);

channel.QueueBind("my.infos", "ex.direct", "info");
channel.QueueBind("my.warnings", "ex.direct", "warning");
channel.QueueBind("my.errors", "ex.direct", "error");


channel.BasicPublish("ex.direct", "info", null, Encoding.UTF8.GetBytes("Message Whit Routing Key Info"));
channel.BasicPublish("ex.direct", "warning", null, Encoding.UTF8.GetBytes("Message Whit Routing Key Warning"));
channel.BasicPublish("ex.direct", "error", null, Encoding.UTF8.GetBytes("Message Whit Routing Key Error"));

