using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Model;

namespace PGenerator.Data;

public class AccountStorageContext : DbContext
{
    public DbSet<AccountInformation> AccountDetails { get; set; }

    public AccountStorageContext(DbContextOptions<AccountStorageContext> options) : base(options) { }
    
    public AccountStorageContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();
            
        optionsBuilder.UseSqlServer(config["ConnectionString"]);
    }
}