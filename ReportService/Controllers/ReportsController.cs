using MeterService.Data;
using MeterService.Entities;
using MeterService.Models;
using Microsoft.AspNetCore.Mvc;
using ReportService.Entities;

namespace MeterService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ReportsController : ControllerBase
  {
    private readonly ReportDbContext _context;

    public ReportsController(ReportDbContext context)
    {
      _context = context;
    }


  }
}
