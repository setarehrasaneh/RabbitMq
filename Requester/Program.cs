using Demo.common;
using Demo.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using Constants = Demo.Common.Constants;

ConcurrentDictionary<string, CalculationRequest> waitingRequests = new ConcurrentDictionary<string, CalculationRequest>();

ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

IConnection con = factory.CreateConnection();
IModel channel = con.CreateModel();

string responseQueueName = "res." + Guid.NewGuid().ToString();
channel.QueueDeclare(responseQueueName);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    string requestId = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers[Constants.RequestIdHeaderKey]);

    CalculationRequest request;
    if(waitingRequests.TryGetValue(requestId, out request))
    {
        string messageData =  Encoding.UTF8.GetString(e.Body.ToArray());
        CalculationResponse response = JsonConvert.DeserializeObject<CalculationResponse>(messageData);
        Console.WriteLine("Calculation Result :" + request.ToString() + "=" + response.ToString());    
    }

    string message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine("Response Received:" + message);
};

channel.BasicConsume(responseQueueName, true, consumer);

Console.WriteLine("Press A Key To Send Request");
Console.ReadKey();

SendRequest(waitingRequests, channel, new CalculationRequest(2,4,OperationType.Add), responseQueueName);
SendRequest(waitingRequests, channel, new CalculationRequest(14,6,OperationType.Subtract), responseQueueName);
SendRequest(waitingRequests, channel, new CalculationRequest(50,2,OperationType.Add), responseQueueName);
SendRequest(waitingRequests, channel, new CalculationRequest(30,6,OperationType.Subtract), responseQueueName);

//while (true)
//{
//    Console.WriteLine("Enter Your Request:");
//    string request = Console.ReadLine();

//    if(request == "exit")
//    {
//        break;
//    }

//    channel.BasicPublish("", "requests", null, Encoding.UTF8.GetBytes(request));
//}
Console.ReadKey(); 
channel.Close();
con.Close();

void SendRequest(
    ConcurrentDictionary<string, CalculationRequest> waitingRequest,
    IModel channel,
    CalculationRequest request,
    string responseQueueName)
{
    string requestId = Guid.NewGuid().ToString();
    string requestData = JsonConvert.SerializeObject(request);
    waitingRequest[requestId] = request;

    var basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>();
    basicProperties.Headers.Add(Constants.RequestIdHeaderKey, Encoding.UTF8.GetBytes(requestId));
    basicProperties.Headers.Add(Constants.ResponseQueueHeaderKey, Encoding.UTF8.GetBytes(responseQueueName));

    channel.BasicPublish(
        "",
        "request",
        basicProperties,
        Encoding.UTF8.GetBytes(requestData));
}