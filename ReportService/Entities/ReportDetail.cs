namespace ReportService.Entities
{
  public class ReportDetail
  {
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public DateTime MeasurementTime { get; set; }
    public decimal LastIndex { get; set; }
    public decimal Voltage { get; set; }
    public decimal Current { get; set; }
    public virtual required Report Report { get; set; }
  }
}
