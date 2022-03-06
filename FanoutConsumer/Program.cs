using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IConnection con = factory.CreateConnection();
IModel channel = con.CreateModel();

var consumer = new EventingBasicConsumer(channel);

consumer.Received += Consumer_Recieved;

var consumerTag = channel.BasicConsume("my.queue1", false, consumer);

Console.WriteLine("Waiting For Messages. press Any Key to Exit.");
Console.ReadKey();



void Consumer_Recieved(object? sender, BasicDeliverEventArgs e)
{
    string message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine("Message:" + message);
    channel.BasicAck(e.DeliveryTag,false);
}