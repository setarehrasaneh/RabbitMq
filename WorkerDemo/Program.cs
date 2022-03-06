using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Console.WriteLine("Enter The Name Of Worker");
string workerName = Console.ReadLine();

ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IConnection con = factory.CreateConnection();
IModel channel = con.CreateModel();

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.ToArray());
    int durationInSeconds = Int32.Parse(message); 
    Console.Write($"[{workerName}] Task Started. Duration: " + durationInSeconds);
    Thread.Sleep(durationInSeconds * 1000);
    Console.WriteLine("FINISHED");

    channel.BasicAck(e.DeliveryTag ,false);
};

var consumerTag = channel.BasicConsume("my.queue1",false,consumer);

Console.WriteLine("Waiting For A Message. Press A Key To Exit.");
Console.ReadKey();

channel.Close();
con.Close();