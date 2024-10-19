namespace ReportService.Models
{
  public class MeterDataDTO
  {
    public DateTime MeasurementTime { get; set; }
    public decimal LastIndex { get; set; }
    public decimal Voltage { get; set; }
    public decimal Current { get; set; }
  }
}
