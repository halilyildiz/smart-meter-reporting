using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MeterService.Controllers;
using MeterService.Data;
using MeterService.Entities;
using MeterService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MeterService.Tests
{
  public class MetersControllerTests
  {
    private readonly MetersController _controller;
    private readonly MeterDbContext _context;

    public MetersControllerTests()
    {

      var options = new DbContextOptionsBuilder<MeterDbContext>()
             .UseInMemoryDatabase(databaseName: "TestMeterDb")
             .Options;

      _context = new MeterDbContext(options);

      _context.Meters.AddRange(new List<Meter>
        {
            new Meter { MeterSerialNumber = "12345678", LastIndex = 100, MeasurementTime = DateTime.Now, Voltage = 210, Current = 30 },
            new Meter { MeterSerialNumber = "12345679", LastIndex = 200, MeasurementTime = DateTime.Now, Voltage = 225, Current = 50 }
        });
      _context.SaveChanges();

      _controller = new MetersController(_context);
    }

    [Fact]
    public void GetLastMeterData_ReturnsOkResult_WhenMeterExists()
    {
      var serialNumber = "12345678";

      var result = _controller.GetLastMeterData(serialNumber);

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnMeter = Assert.IsType<Meter>(okResult.Value);
      Assert.Equal(serialNumber, returnMeter.MeterSerialNumber);
    }

    [Fact]
    public void GetLastMeterData_ReturnsNotFound_WhenMeterDoesNotExist()
    {
      var serialNumber = "12345";

      var result = _controller.GetLastMeterData(serialNumber);

      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task SaveMeterData_ReturnsCreatedAtActionResult_WhenDataIsValid()
    {
      var meterRequest = new MeterMeasurementRequest
      {
        MeterSerialNumber = "23456789",
        LastIndex = 100,
        Current = 50,
        Voltage = 230,
        MeasurementTime = System.DateTime.Now
      };


      var result = await _controller.SaveMeterData(meterRequest);

      var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
      var meter = Assert.IsType<Meter>(createdAtActionResult.Value);
      Assert.Equal(meterRequest.MeterSerialNumber, meter.MeterSerialNumber);
    }

    [Fact]
    public async Task SaveMeterData_ReturnsBadRequest_WhenRequestIsNull()
    {
      var result = await _controller.SaveMeterData(null);

      Assert.IsType<BadRequestResult>(result.Result);
    }
  }
}