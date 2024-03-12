using DatabaseLibrary.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PGenerator.Data;

public class StorageContext : DbContext
{
    public DbSet<Information> Information { get; set; }

    public StorageContext(DbContextOptions<StorageContext> options) : base(options) { }
    
    public StorageContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();
            
        optionsBuilder.UseSqlServer(config["ConnectionString"]);
    }
}