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

  public class ReportProcessingService : BackgroundService
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly MeterServiceHttpClient _meterService;

    public ReportProcessingService(MeterServiceHttpClient meterService, IServiceProvider serviceProvider)
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
      _meterService = meterService;
      _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      var consumer = new EventingBasicConsumer(_channel);
      consumer.Received += (model, ea) =>
      {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var cleanedMessage = message.Trim('"');

        Guid.TryParse(cleanedMessage, out Guid reportId);

        ProcessReport(reportId);
      };

      _channel.BasicConsume(queue: "reportQueue",
                           autoAck: true,
                           consumer: consumer);

      return Task.CompletedTask;
    }

    private async Task ProcessReport(Guid reportId)
    {
      using (var scope = _serviceProvider.CreateScope())
      {
        var _context = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        var report = _context.Reports.FirstOrDefault(x => x.Id == reportId);
        if (report != null)
        {
          var meterDatas = await _meterService.GetMeterDataAsync(report.MeterSerialNumber);

          if (meterDatas != null && meterDatas.Count > 0)
          {

            var fileName = $"MeterReport_{report.MeterSerialNumber}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var filePath = Path.Combine("Reports", fileName);

            if (!Directory.Exists("Reports"))
            {
              Directory.CreateDirectory("Reports");
            }

            using (var writer = new StreamWriter(filePath))
            {
              writer.WriteLine($"Serial Number: {report.MeterSerialNumber}");
              writer.WriteLine("");
              writer.WriteLine("MeasurementTime || Last Index || Voltage || Current");
              writer.WriteLine("-----------------------------------------------------");
              foreach (var meterData in meterDatas.OrderByDescending(x => x.MeasurementTime))
              {
                writer.WriteLine($"{meterData.MeasurementTime} || {meterData.LastIndex}  ||  {meterData.Voltage}  || {meterData.Current}");
              }
            }

            report.Status = (int)EReportState.Completed;
            report.DocumentPath = filePath;
            await _context.SaveChangesAsync();
          }
        }
      }
    }

    public override void Dispose()
    {
      _channel.Close();
      _connection.Close();
      base.Dispose();
    }
  }
}
