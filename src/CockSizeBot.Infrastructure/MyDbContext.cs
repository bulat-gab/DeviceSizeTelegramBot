using Microsoft.EntityFrameworkCore;

namespace CockSizeBot.Infrastructure;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Measurement> Measurements { get; set; }

    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,20030;Initial Catalog=devicesize-main-db;User ID=sa;Password=Your_password123");
        }
    }
}
