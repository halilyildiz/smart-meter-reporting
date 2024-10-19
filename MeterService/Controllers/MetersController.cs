using MeterService.Data;
using MeterService.Entities;
using MeterService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeterService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MetersController : ControllerBase
  {
    private readonly MeterDbContext _context;

    public MetersController(MeterDbContext context)
    {
      _context = context;
    }

    [HttpGet("GetLastMeterData/{serialNumber}")]
    public ActionResult<Meter> GetLastMeterData(string serialNumber)
    {
      var meter = _context.Meters
          .Where(m => m.MeterSerialNumber == serialNumber)
          .OrderByDescending(m => m.MeasurementTime)
          .FirstOrDefault();

      if (meter == null)
      {
        return NotFound();
      }

      return Ok(meter);
    }

    [HttpGet("{serialNumber}")]
    public ActionResult<List<Meter>> GetMeterDatas(string serialNumber)
    {
      var meters = _context.Meters
          .Where(m => m.MeterSerialNumber == serialNumber)
          .OrderByDescending(m => m.MeasurementTime)
          .ToList();

      if (meters == null)
      {
        return NotFound();
      }

      return Ok(meters);
    }

    [HttpPost]
    public async Task<ActionResult<Meter>> SaveMeterData([FromBody] MeterMeasurementRequest meterRequest)
    {
      if (meterRequest == null)
      {
        return BadRequest();
      }

      var meter = new Meter() 
      { 
        MeterSerialNumber = meterRequest.MeterSerialNumber, 
        LastIndex = meterRequest.LastIndex, 
        Current = meterRequest.Current, 
        Voltage = meterRequest.Voltage, 
        MeasurementTime = meterRequest.MeasurementTime 
      };

      _context.Meters.Add(meter);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetLastMeterData), new { serialNumber = meter.MeterSerialNumber }, meter);
    }

  }
}
