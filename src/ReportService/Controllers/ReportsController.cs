
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Data;
using ReportService.Entities;
using ReportService.Enumerations;
using ReportService.Producers;

namespace ReportService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ReportsController : ControllerBase
  {
    private readonly ReportDbContext _context;
    private readonly ReportRequestProducer _reportRequestProducer;

    public ReportsController(ReportDbContext context, ReportRequestProducer reportRequestProducer)
    {
      _context = context;
      _reportRequestProducer = reportRequestProducer;
    }

    [HttpGet()]
    public ActionResult GetReports()
    {
      var reports = _context.Reports.AsNoTracking().ToList();


      if (reports == null)
      {
        return NotFound();
      }

      return Ok(reports);
    }

    [HttpGet("{serialNumber}")]
    public ActionResult GetReport(string serialNumber)
    {  
      var report = _context.Reports
          .Where(x => x.MeterSerialNumber == serialNumber && x.Status == (int)EReportState.Completed)
          .OrderByDescending(x => x.RequestedAt)
          .FirstOrDefault();

      if (report == null)
      {
        return NotFound();
      }

      var filePath = Path.Combine(Directory.GetCurrentDirectory(), report.DocumentPath); 
      if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
      {
        return NotFound("File not found");
      }

      var contentType = "application/octet-stream";
      var fileName = Path.GetFileName(filePath); 
      return PhysicalFile(filePath, contentType, fileName);
    }

    [HttpPost("{serialNumber}")]
    public ActionResult RequestReport(string serialNumber)
    {
      if (string.IsNullOrEmpty(serialNumber))
      {
        return BadRequest();
      }

      var report = new Report()
      {
        MeterSerialNumber = serialNumber,
        RequestedAt = DateTime.Now,
        Status = (int)EReportState.Preparation,
        DocumentPath = ""
      };

      _context.Add(report);
      _context.SaveChanges();

      _reportRequestProducer.SendReportRequest(report.Id);

      return Ok(report);
    }



  }
}
