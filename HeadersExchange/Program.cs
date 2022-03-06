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
    "ex.headers",
    ExchangeType.Headers,
    true,
    false,
    null);

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

channel.QueueBind(
    "my.queue1",
    "ex.headers",
    "",
    new Dictionary<string, object>
    {
        {"x-match", "all"},
        {"job","convert"},
        {"format","jpeg"}
    });

channel.QueueBind(
    "my.queue2",
    "ex.headers",
    "",
    new Dictionary<string, object>
    {
        {"x-match", "any"},
        {"job","convert"},
        {"format","jpeg"}
    });

 IBasicProperties props = channel.CreateBasicProperties();
props.Headers = new Dictionary<string, object>();
props.Headers.Add("job", "convert");
props.Headers.Add("format", "jpeg");

channel.BasicPublish(
    "ex.headers",
    "",
    props,
    Encoding.UTF8.GetBytes("Message1"));

props = channel.CreateBasicProperties();
props.Headers = new Dictionary<string, object>();
props.Headers.Add("job", "convert");
props.Headers.Add("format", "bmp");

channel.BasicPublish(
    "ex.headers",
    "",
    props,
    Encoding.UTF8.GetBytes("Message2"));



