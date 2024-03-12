using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Data;
using Microsoft.Extensions.Configuration;

namespace PGenerator;

public partial class App : Application
{
    public static UsersContext UserDb { get; set; }
    public static StorageContext StorageDb { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .Build();

        var usersContextBuilder = new DbContextOptionsBuilder<UsersContext>();
        usersContextBuilder.UseSqlServer(config["ConnectionString"]);
        UserDb = new UsersContext(usersContextBuilder.Options);
        UserDb.Database.Migrate();
        
        var storageContextBuilder = new DbContextOptionsBuilder<StorageContext>();
        storageContextBuilder.UseSqlServer(config["ConnectionString"]);
        StorageDb = new StorageContext(storageContextBuilder.Options);
        StorageDb.Database.Migrate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        UserDb.Dispose();
        base.OnExit(e);
    }
}