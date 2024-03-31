using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Model;

namespace PGenerator.Data;

public class UsersContext : IdentityDbContext<UserInformation, IdentityRole, string>
{
    public UsersContext() { }
    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }
    
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