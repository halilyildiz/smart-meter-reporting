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
  using ReportService.ReportBO;

  public class ReportProcessingService : BackgroundService
  {
    private readonly IReportProcessor _reportProcessor;
    private readonly ReportRequestConsumer _reportRequestConsumer;

    public ReportProcessingService(IReportProcessor reportProcessor, ReportRequestConsumer reportRequestConsumer)
    {
      _reportProcessor = reportProcessor;
      _reportRequestConsumer = reportRequestConsumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _reportRequestConsumer.Consume(async (message) =>
      {
        if (Guid.TryParse(message, out Guid reportId))
        {
          await _reportProcessor.ProcessReportAsync(reportId);
        }
      });

      return Task.CompletedTask;
    }
  }
}
