using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
  public class MeterController : Controller
  {
    private readonly MeterService _meterService;

    public MeterController(MeterService meterService)
    {
      _meterService = meterService;
    }

    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> GetLastMeterData(string serialNumber)
    {
      var meterData = await _meterService.GetLastMeterDataAsync(serialNumber);

      if (meterData == null)
      {
        return NotFound("Veri bulunamadı.");
      }

      return PartialView("Partials/_meterPartial", meterData);
    }

    public async Task<IActionResult> SaveMeterData([FromBody] MeterRequest meterRequest)
    {
      var newMeterData = await _meterService.SaveMeterDataAsync(meterRequest);

      return PartialView("Partials/_meterPartial", newMeterData);
    }
  }
}
