namespace WebUI.Models
{
  public class MeterResponse
  {
    public Guid Id { get; set; }
    public required string MeterSerialNumber { get; set; }
    public DateTime MeasurementTime { get; set; }
    public decimal LastIndex { get; set; }
    public decimal Voltage { get; set; }
    public decimal Current { get; set; }
  }
}
