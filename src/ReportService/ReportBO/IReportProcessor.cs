namespace ReportService.ReportBO
{
  public interface IReportProcessor
  {
    Task ProcessReportAsync(Guid reportId);
  }
}
