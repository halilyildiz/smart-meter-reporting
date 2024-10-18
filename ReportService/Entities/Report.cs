namespace ReportService.Entities
{
  public class Report
  {
    public Report()
    {
      ReportDetails = new HashSet<ReportDetail>();
    }
    public Guid Id { get; set; }
    public required string MeterSerialNumber { get; set; }
    public DateTime RequestedAt { get; set; }
    public int Status { get; set; }
    public ICollection<ReportDetail> ReportDetails { get; set; }
  }

}
