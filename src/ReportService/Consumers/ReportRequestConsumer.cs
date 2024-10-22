namespace ReportService.BackgroundServices
{
  using Microsoft.Extensions.Hosting;
  using RabbitMQ.Client;
  using RabbitMQ.Client.Events;
  using System;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Diagnostics;
  using ReportService.Services;
  using ReportService.Data;
  using Microsoft.EntityFrameworkCore;
  using ReportService.Enumerations;

  public class ReportRequestConsumer 
  {
    private readonly IModel _channel;
    public ReportRequestConsumer(IConnectionFactory connectionFactory)
    {
      var connection = connectionFactory.CreateConnection();
      _channel = connection.CreateModel();
      _channel.QueueDeclare(queue: "reportQueue",
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);
    }

    public void Consume(Func<string, Task> onMessageReceived)
    {
      var consumer = new EventingBasicConsumer(_channel);
      consumer.Received += async (model, ea) =>
      {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var cleanedMessage = message.Trim('"');
        await onMessageReceived(cleanedMessage);
      };

      _channel.BasicConsume(queue: "reportQueue",
                           autoAck: true,
                           consumer: consumer);
    }

    public void Dispose()
    {
      _channel.Close();
      _channel.Dispose();
    }
  }
}
