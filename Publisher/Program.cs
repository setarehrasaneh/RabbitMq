

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

while (true)
{
    Console.WriteLine("Enter Message");
    string message = Console.ReadLine();

    if (message == "exit")
    {
        break;
    }
    channel.BasicPublish("ex.fanout", "", null, Encoding.UTF8.GetBytes(message));
    channel.Close();
    con.Close();    
}