namespace WebUI.Models
{
  public class ReportResponse
  {
    public Guid Id { get; set; }
    public string MeterSerialNumber { get; set; }
    public DateTime RequestedAt { get; set; }
    public int Status { get; set; }
  }
}
