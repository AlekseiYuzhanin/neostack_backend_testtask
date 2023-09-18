using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class BaseContext : DbContext
{
    private readonly IConfiguration _config;

    public BaseContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Skill> Skills { get; set; }
  
}