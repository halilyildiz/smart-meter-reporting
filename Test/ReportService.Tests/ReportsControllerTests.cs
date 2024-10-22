using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ReportService.Controllers;
using ReportService.Data;
using ReportService.Entities;
using ReportService.Enumerations;
using ReportService.Producers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ReportService.Tests
{
  public class ReportsControllerTests
  {
    private readonly ReportsController _controller;
    private readonly ReportDbContext _context;

    public ReportsControllerTests()
    {
      var options = new DbContextOptionsBuilder<ReportDbContext>()
          .UseInMemoryDatabase(databaseName: "TestReportDb")
          .Options;

      _context = new ReportDbContext(options);

      _context.Reports.AddRange(new List<Report>
            {
                new Report { Id = Guid.NewGuid(), MeterSerialNumber = "12345678", Status = (int)EReportState.Completed, DocumentPath = "path/to/report1.pdf" },
                new Report { Id = Guid.NewGuid(), MeterSerialNumber = "45678901", Status = (int)EReportState.Preparation, DocumentPath = "path/to/report2.pdf" },
                new Report { Id = Guid.NewGuid(), MeterSerialNumber = "56789012", Status = (int)EReportState.Preparation, DocumentPath = "path/to/report2.pdf" }
            });
      _context.SaveChanges();

      var factory = new ConnectionFactory
      {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest",
        Port = 5672,
        VirtualHost = "/"
      };

      var reportRequestProducer = new ReportRequestProducer(factory);
      _controller = new ReportsController(_context, reportRequestProducer);
    }

    [Fact]
    public void GetReports_ShouldReturnAllReports()
    {
      var result = _controller.GetReports();

      var okResult = Assert.IsType<OkObjectResult>(result);
      var reports =  Assert.IsType<List<Report>>(okResult.Value);
      Assert.NotNull(reports);
    }

    [Fact]
    public void GetReport_ShouldReturnNotFound_WhenReportDoesNotExist()
    {
      var result = _controller.GetReport("13544544");

      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void RequestReport_ShouldReturnOk_WhenValidSerialNumber()
    {
      string serialNumber = "12345678";

      var result = _controller.RequestReport(serialNumber);

      var okResult = Assert.IsType<OkObjectResult>(result);
      var report = Assert.IsType<Report>(okResult.Value);
      Assert.Equal(serialNumber, report.MeterSerialNumber);
    }

    [Fact]
    public void RequestReport_ShouldReturnBadRequest_WhenSerialNumberIsNull()
    {
      var result = _controller.RequestReport(null);

      Assert.IsType<BadRequestResult>(result);
    }
  }
}
