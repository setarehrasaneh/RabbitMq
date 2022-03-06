using Demo.common;
using Demo.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Constants = Demo.Common.Constants;

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
    string requestData = Encoding.UTF8.GetString(e.Body.ToArray());
    CalculationRequest request = JsonConvert.DeserializeObject<CalculationRequest>(requestData);
    Console.WriteLine("Request Received" + request.ToString());
    
    CalculationResponse response = new CalculationResponse();

    if(request.Operation == OperationType.Add)
    {
        response.Result = request.Num1 + request.Num2;
    }
    else if (request.Operation == OperationType.Subtract)
    {
        response.Result = request.Num1 - request.Num2;
    }
    string responseData = JsonConvert.SerializeObject(response);

    var basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>();
    basicProperties.Headers.Add(Constants.RequestIdHeaderKey, e.BasicProperties.Headers[Constants.RequestIdHeaderKey]);

    string responseQueueName = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers[Constants.ResponseQueueHeaderKey]);

    channel.BasicPublish("", responseQueueName, basicProperties, Encoding.UTF8.GetBytes(responseData));
};

channel.BasicConsume("request", true, consumer);

Console.WriteLine("Press A Key To Exit.");
Console.ReadKey();
channel.Close();
con.Close();