
using ReportService.Data;
using ReportService.Enumerations;
using ReportService.Services;

namespace ReportService.ReportBO
{
  public class ReportProcessor : IReportProcessor
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly MeterServiceHttpClient _meterService;

    public ReportProcessor(MeterServiceHttpClient meterService, IServiceProvider serviceProvider)
    {
      _meterService = meterService;
      _serviceProvider = serviceProvider;
    }
    public async Task ProcessReportAsync(Guid reportId)
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
  }
}
