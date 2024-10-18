﻿// <auto-generated />
using System;
using MeterService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ReportService.Migrations
{
    [DbContext(typeof(ReportDbContext))]
    partial class ReportDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ReportService.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MeterSerialNumber")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<DateTime>("RequestedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("ReportService.Entities.ReportDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Current")
                        .HasColumnType("decimal(8,2)");

                    b.Property<decimal>("LastIndex")
                        .HasColumnType("decimal(8,2)");

                    b.Property<DateTime>("MeasurementTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Voltage")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportDetails");
                });

            modelBuilder.Entity("ReportService.Entities.ReportDetail", b =>
                {
                    b.HasOne("ReportService.Entities.Report", "Report")
                        .WithMany("ReportDetails")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");
                });

            modelBuilder.Entity("ReportService.Entities.Report", b =>
                {
                    b.Navigation("ReportDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
