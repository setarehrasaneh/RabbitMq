// See https://aka.ms/new-console-template for more information

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

channel.QueueDeclare(
    "my.queue1",
    true,
    false,
    false,
    null
    );

channel.QueueDeclare(
    "my.queue2",
    true,
    false,
    false,
    null
    );

channel.QueueBind("my.queue1", "ex.fanout", "");
channel.QueueBind("my.queue2", "ex.fanout", "");

channel.BasicPublish("ex.fanout", "", null, Encoding.UTF8.GetBytes("Message 1"));
channel.BasicPublish("ex.fanout", "", null, Encoding.UTF8.GetBytes("Message 2"));

Console.WriteLine("Press Any Key For Exit");
Console.ReadKey();

channel.QueueDelete("my.queue1");
channel.QueueDelete("my.queue2");

channel.ExchangeDelete("ex.fanout");

channel.Close();
con.Close();    
 



