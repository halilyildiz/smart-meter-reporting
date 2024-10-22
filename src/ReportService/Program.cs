using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ReportService.BackgroundServices;
using ReportService.Data;
using ReportService.Producers;
using ReportService.ReportBO;
using ReportService.Services;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpClient<MeterServiceHttpClient>();
builder.Services.AddHostedService<ReportProcessingService>();

builder.Services.AddSingleton<ReportRequestProducer>();
builder.Services.AddSingleton<ReportRequestConsumer>();
builder.Services.AddSingleton<IReportProcessor, ReportProcessor>();

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");

builder.Services.AddSingleton<IConnectionFactory>(sp => {
  var factory = new ConnectionFactory
  {
    HostName = rabbitMqSettings["HostName"],
    UserName = rabbitMqSettings["UserName"],
    Password = rabbitMqSettings["Password"],
    Port = int.Parse(rabbitMqSettings["Port"] ?? ""),
    VirtualHost = rabbitMqSettings["VirtualHost"]
  };
  return factory;
});


builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
