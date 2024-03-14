using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Model;

namespace PGenerator.Data;

public class UsersContext : IdentityDbContext<User, IdentityRole, string>
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
    
    public UsersContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();
            
            optionsBuilder.UseSqlServer(config["ConnectionString"]);
        }
    }
}