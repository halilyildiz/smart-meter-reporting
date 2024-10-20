namespace WebUI.Models
{
  public class MeterRequest
  {
    public required string MeterSerialNumber { get; set; }
    public DateTime MeasurementTime { get; set; }
    public decimal LastIndex { get; set; }
    public decimal Voltage { get; set; }
    public decimal Current { get; set; }
  }
}
