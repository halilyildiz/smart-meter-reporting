using MeterService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MeterService.Data
{
  public class MeterDbContext : DbContext
  {
    public MeterDbContext(DbContextOptions<MeterDbContext> options) : base(options) { }

    public DbSet<Meter> Meters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Meter>()
        .HasKey(e => e.Id);

      modelBuilder.Entity<Meter>()
        .Property(x => x.MeterSerialNumber)
        .HasMaxLength(8);
          
      modelBuilder.Entity<Meter>()
        .Property(x => x.Voltage)
        .HasColumnType("decimal(8,2)");

      modelBuilder.Entity<Meter>()
        .Property(x => x.Current)
        .HasColumnType("decimal(8,2)");

      modelBuilder.Entity<Meter>()
        .Property(x => x.LastIndex)
        .HasColumnType("decimal(8,2)");
    }
  }
}
