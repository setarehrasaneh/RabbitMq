


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
    "ex.topic",
    ExchangeType.Topic,
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

channel.QueueDeclare(
    "my.queue3",
    true,
    false,
    false,
    null);

channel.QueueBind("my.queue1", "ex.topic", "*.image.*");
channel.QueueBind("my.queue2", "ex.topic", "#.image");
channel.QueueBind("my.queue3", "ex.topic", "image.#");


channel.BasicPublish(
    "ex.topic",
    "convert.image.bitmap",
    null,
    Encoding.UTF8.GetBytes("Routing Key is convert.image.bitmap"));

channel.BasicPublish(
    "ex.topic",
    "convert.bitmap.image",
    null,
    Encoding.UTF8.GetBytes("Routing Key is convert.bitmap.image"));

channel.BasicPublish(
    "ex.topic",
    "image.bitmap.32bit",
    null,
    Encoding.UTF8.GetBytes("Routing Key is image.bitmap.32bit"));