using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GestionHotel.Apis.Models;

public partial class ApiContext : DbContext
{
    private readonly IConfiguration _configuration;
    public ApiContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DBConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07F30E8806");

            entity.ToTable("reservation");

            entity.Property(e => e.EndDate).HasColumnName("End_Date");
            entity.Property(e => e.Paid)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RoomId).HasColumnName("Room_Id");
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.TotalPrice).HasColumnName("Total_Price");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07F5FFD3A2");

            entity.ToTable("room");

            entity.Property(e => e.Cleaned)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NbPeople).HasColumnName("Nb_People");
            entity.Property(e => e.Occupied)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07CE5C9DF5");

            entity.ToTable("user");

            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}