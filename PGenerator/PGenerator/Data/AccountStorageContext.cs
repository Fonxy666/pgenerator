using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Model;

namespace PGenerator.Data;

public class AccountStorageContext : DbContext
{
    public DbSet<AccountDetail> AccountDetails { get; set; }
    public AccountStorageContext() { }
    public AccountStorageContext(DbContextOptions<AccountStorageContext> options) : base(options) { }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();
            
        optionsBuilder.UseSqlServer(config["ConnectionString"]);
    }
}