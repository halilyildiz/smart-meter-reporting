namespace ReportService.Entities
{
  public class Report
  {
    public Guid Id { get; set; }
    public required string MeterSerialNumber { get; set; }
    public DateTime RequestedAt { get; set; }
    public int Status { get; set; }
    public  string DocumentPath { get; set; }
  }
}
