using Microsoft.EntityFrameworkCore;
using ReportService.Entities;
using System.Reflection.Metadata;

namespace MeterService.Data
{
  public class ReportDbContext : DbContext
  {
    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportDetail> ReportDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Report>()
        .HasKey(e => e.Id);

      modelBuilder.Entity<Report>()
        .HasMany(x => x.ReportDetails)
        .WithOne(x => x.Report)
        .HasForeignKey(x => x.ReportId);

      modelBuilder.Entity<Report>()
        .Property(x => x.MeterSerialNumber)
        .HasMaxLength(8);

      modelBuilder.Entity<ReportDetail>()
        .HasKey(e => e.Id);
          
      modelBuilder.Entity<ReportDetail>()
        .Property(x => x.Voltage)
        .HasColumnType("decimal(8,2)");

      modelBuilder.Entity<ReportDetail>()
        .Property(x => x.Current)
        .HasColumnType("decimal(8,2)");

      modelBuilder.Entity<ReportDetail>()
        .Property(x => x.LastIndex)
        .HasColumnType("decimal(8,2)");

    
    }
  }
}
