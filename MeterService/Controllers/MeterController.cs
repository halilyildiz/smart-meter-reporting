using MeterService.Data;
using MeterService.Entities;
using MeterService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeterService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MeterController : ControllerBase
  {
    private readonly MeterDbContext _context;

    public MeterController(MeterDbContext context)
    {
      _context = context;
    }

    [HttpGet("{serialNumber}")]
    public ActionResult<Meter> GetLastMeasurement(string serialNumber)
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

    [HttpPost]
    public async Task<ActionResult<Meter>> SaveMeasurement([FromBody] MeterMeasurementRequest meterRequest)
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

      return CreatedAtAction(nameof(GetLastMeasurement), new { serialNumber = meter.MeterSerialNumber }, meter);
    }

  }
}
