using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace electricity;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultContainer("electricity");
        modelBuilder.Entity<Location>().ToContainer("Locations");
        modelBuilder.Entity<Location>().Property(b => b.Id).HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Location>().HasNoDiscriminator();
    }

    public DbSet<Location> Locations { get; set; }
}

[Table("locations")]
public class Location
{

    public Guid Id { get; set; }
    public DateTime LastAlive { get; set; }
    public bool IsOnline { get; set; }
}


