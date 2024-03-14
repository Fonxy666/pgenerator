using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PGenerator.Service.UserManager;

namespace PGenerator;

public partial class App : Application
{
    public static UsersContext UserDb { get; set; }
    public static StorageContext StorageDb { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddDbContext<UsersContext>(options =>
                options.UseSqlServer(config["ConnectionString"]))
            .AddDbContext<StorageContext>(options =>
                options.UseSqlServer(config["ConnectionString"]))
            .AddSingleton<UserService>()
            .BuildServiceProvider();
        
        UserDb = serviceProvider.GetRequiredService<UsersContext>();
        StorageDb = serviceProvider.GetRequiredService<StorageContext>();

        UserDb.Database.Migrate();
        StorageDb.Database.Migrate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        UserDb.Dispose();
        base.OnExit(e);
    }
}