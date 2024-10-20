using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
  public class ReportController : Controller
  {
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
      _reportService = reportService;
    }

    public async Task<IActionResult> Index()
    {
      var reports = await _reportService.GetReportsAsync();
      return View(reports);
    }

    //public async Task<IActionResult> GetAllReports()
    //{
    //  var reports = await _reportService.GetReportsAsync();
    //  return PartialView(reports);
    //}

    //public async Task<IActionResult> GetReportDetails(string serialNumber)
    //{
    //  var report = await _reportService.GetReportAsync(serialNumber);
    //  return report != null ? View(report) : NotFound();
    //}

    public async Task<IActionResult> RequestNewReport(string serialNumber)
    {
      var isRequest = await _reportService.RequestReportAsync(serialNumber);
      return Ok(isRequest ? "Raport talebi başarıyla alındı" : "Rapor talebi oluşturulamadı.");
    }

    public async Task<IActionResult> DownloadReport(Guid reportId)
    {
      var reportStream = await _reportService.GetReportAsync(reportId);

      if (reportStream == null)
      {
        return NotFound("Rapor bulunamadı.");
      }

      var fileName = $"rapor.txt";
      return File(reportStream, "application/octet-stream", fileName);
    }
  }
}
