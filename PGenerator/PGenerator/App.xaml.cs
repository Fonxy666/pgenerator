using System.Windows;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGenerator.Model;
using PGenerator.Service.UserManager;
using PGenerator.View;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace PGenerator;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    
    public App()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<App>();

        var configuration = builder.Build();
        
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContent, services) =>
            {
                services.AddScoped<IUserService, UserService>();
                services.AddDbContext<UsersContext>(options =>
                    options.UseSqlServer(configuration["ConnectionString"]));
                services.AddDbContext<StorageContext>(options =>
                    options.UseSqlServer(configuration["ConnectionString"]));
                services.AddScoped<LoginWindow>();
                services.AddScoped<Registration>();
                services.AddIdentityCore<UserInformation>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireLowercase = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UsersContext>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        var serviceProvider = AppHost.Services;

        var registerWindow = serviceProvider.GetService<LoginWindow>();

        registerWindow!.UserService = serviceProvider.GetService<IUserService>()!;

        MainWindow = registerWindow;
        registerWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}