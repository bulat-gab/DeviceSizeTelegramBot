using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CockSizeBot.Infrastructure;

public class MyDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DbSet<User> Users { get; set; }

    public DbSet<Measurement> Measurements { get; set; }

    public MyDbContext(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=tcp:devicesizebot.database.windows.net,1433;Initial Catalog=devicesize-main-db;Persist Security Info=False;User ID=devicesizebot;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }
}
