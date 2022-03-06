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

ReadMessagesWithPushModel();

ReadMessagesWithPullModel();


channel.Close();    
con.Close();




 void ReadMessagesWithPushModel()
{
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, e) =>
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine("Message :" + message);
    };

    string consumerTag = channel.BasicConsume("my.queue1",true,consumer);

    Console.WriteLine("Subscribed. Press Any Key To Unsubscribe And Exit");
    Console.ReadKey();

    channel.BasicCancel(consumerTag);
}

void ReadMessagesWithPullModel()
{
    Console.WriteLine("Reading Message From Queue. Press e To Exit.");

    while (true)
    {
        Console.WriteLine("Trying To Get A Message From Queue.....");
        BasicGetResult resul = channel.BasicGet("my.queue1", true);
        
        if(resul != null)
        {
            string message = Encoding.UTF8.GetString(resul.Body.ToArray());
        }

        if (Console.KeyAvailable)
        {
            var keyInfo = Console.ReadKey();
            if (keyInfo.KeyChar == 'e' || keyInfo.KeyChar == 'E')
            {
                return;
            }
            Thread.Sleep(2000);
        }
    }
}