using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Serilog;

public class DatabaseContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.Equipment).HasDefaultValue(new string[] { });
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now', 'utc')");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Organizer).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now', 'utc')");
            entity.HasOne(e => e.Room)
                  .WithMany(r => r.Reservations)
                  .HasForeignKey(e => e.RoomId);
        });
    }
}

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string[] Equipment { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}

public class Reservation
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public string Organizer { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class Database
{
    private const string ConnectionString = "Data Source=academy.db";

    public static async Task SetupDatabaseAsync()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(ConnectionString)
            .Options;

        using var context = new DatabaseContext(options);
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while setting up the database.");
            throw;
        }
    }
}