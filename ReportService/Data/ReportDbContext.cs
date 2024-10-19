using Microsoft.EntityFrameworkCore;
using ReportService.Entities;
using System.Reflection.Metadata;

namespace ReportService.Data
{
  public class ReportDbContext : DbContext
  {
    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Report>()
        .HasKey(e => e.Id);

      modelBuilder.Entity<Report>()
        .Property(e => e.MeterSerialNumber)
        .HasMaxLength(8);
    }
  }
}
