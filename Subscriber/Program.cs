
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Enter The Queue Name");
string queueName = Console.ReadLine();
ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IConnection con = factory.CreateConnection();
IModel channel = con.CreateModel();

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
   string message = Encoding.UTF8.GetString(e.Body.ToArray());
   Console.WriteLine("Subcriber [" + queueName + "] message: " + message);
};

var consumerTag = channel.BasicConsume(queueName, true, consumer);

Console.WriteLine("Subscribe to The Queue '{ queueName }'. Press A Key To Exit");
Console.ReadKey();

channel.Close();
con.Close();
