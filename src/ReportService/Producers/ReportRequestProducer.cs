using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace ReportService.Producers
{
  public class ReportRequestProducer
  {
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public ReportRequestProducer()
    {
      var factory = new ConnectionFactory()
      {
        HostName = "localhost",      
        Port = 5672,                 
        UserName = "guest",          
        Password = "guest"          
      };

      _connection = factory.CreateConnection();
      _channel = _connection.CreateModel();
      _channel.QueueDeclare(queue: "reportQueue",
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);
    }

    public void SendReportRequest(Guid reportId)
    {
      var message = JsonSerializer.Serialize(reportId);
      var body = Encoding.UTF8.GetBytes(message);

      _channel.BasicPublish(exchange: "",
                           routingKey: "reportQueue",
                           basicProperties: null,
                           body: body);
    }
  }
}
