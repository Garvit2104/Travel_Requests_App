using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Travel_Requests_App.Models;

namespace Travel_Requests_App.Data;

public partial class TravelPlannerDbContext : DbContext
{
    public TravelPlannerDbContext()
    {
    }

    public TravelPlannerDbContext(DbContextOptions<TravelPlannerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<TravelBudgetAllocation> TravelBudgetAllocations { get; set; }

    public virtual DbSet<TravelRequest> TravelRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TravelPlanner_DB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3213E83F2C99D138");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TravelBudgetAllocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelBu__3213E83FC86FC8D7");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovedBudget).HasColumnName("approved_budget");
            entity.Property(e => e.ApprovedHotelStarRating)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("approved_hotel_star_rating");
            entity.Property(e => e.ApprovedModeOfTravel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("approved_mode_of_travel");
            entity.Property(e => e.TravelRequestId).HasColumnName("travel_request_id");

            entity.HasOne(d => d.TravelRequest).WithMany(p => p.TravelBudgetAllocations)
                .HasForeignKey(d => d.TravelRequestId)
                .HasConstraintName("FK__TravelBud__appro__4BAC3F29");
        });

        modelBuilder.Entity<TravelRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__TravelRe__18D3B90F53EAD1BF");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Priority)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PurposeOfTravel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("purpose_of_travel");
            entity.Property(e => e.RaisedByEmployeeId).HasColumnName("raised_by_employee_id");
            entity.Property(e => e.RequestRaisedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("request_raised_on");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("request_status");
            entity.Property(e => e.ToBeApprovedByHrId).HasColumnName("to_be_approved_by_hr_id");
            entity.Property(e => e.ToDate).HasColumnName("to_date");

            entity.HasOne(d => d.Location).WithMany(p => p.TravelRequests)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__TravelReq__locat__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
