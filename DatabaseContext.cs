using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace electricity;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Location>().ToContainer("Locations");
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


