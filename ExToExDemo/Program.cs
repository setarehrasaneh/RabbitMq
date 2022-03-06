



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
    "exchange1",
    ExchangeType.Direct,
    true,
    false,
    null);

channel.ExchangeDeclare(
    "exchange2",
    ExchangeType.Direct,
    true,
    false,
    null);

channel.QueueDeclare(
    "exchange1",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "exchange2",
    true,
    false,
    false,
    null);

channel.QueueBind("my.queue1", "exchange1", "key1");
channel.QueueBind("my.queue2", "exchange2", "key2");

channel.ExchangeBind("exchange2", "exchange1", "key2");

channel.BasicPublish(
    "exchange1",
     "key1",
     null,
     Encoding.UTF8.GetBytes("Message With Routing Key Key1"));

channel.BasicPublish(
    "exchange1",
     "key2",
     null,
     Encoding.UTF8.GetBytes("Message With Routing Key Key2"));