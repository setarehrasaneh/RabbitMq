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
    "ex.fanout",
    ExchangeType.Fanout,
    true,
    false,
    null);

channel.ExchangeDeclare(
    "ex.direct",
    ExchangeType.Direct,
    true,
    false,
    new Dictionary<string, object>
    {
        {"alternate-exchange","ex.fanout" }
    });

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

channel.QueueDeclare(
    "my.unrouted",
    true,
    false,
    false,
    null);

channel.QueueBind("my.queue1","ex.direct","video",null);
channel.QueueBind("my.queue2","ex.direct","image",null);
channel.QueueBind("my.unrouted", "ex.fanout","", null);


IBasicProperties props = channel.CreateBasicProperties();
props.Headers = new Dictionary<string, object>();
props.Headers.Add("job", "convert");
props.Headers.Add("format", "jpeg");


channel.BasicPublish(
    "ex.direct",
    "video",
    null,
    Encoding.UTF8.GetBytes("Message With Routhing Key Video"));

channel.BasicPublish(
    "ex.direct",
    "text",
    null,
    Encoding.UTF8.GetBytes("Message With Routhing Key Video"));



