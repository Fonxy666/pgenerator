using System.Text;
using System.Windows;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGenerator.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PGenerator.Model;
using PGenerator.Service.AuthService;
using PGenerator.Service.InformationService;
using PGenerator.Service.UserManager;
using PGenerator.TokenStorageFolder;
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
        var connectionString = configuration["ConnectionString"];
        var issueAudience = configuration["IssueAudience"];
        var issueSign = configuration["IssueSign"];
        
        var dataProtectionProvider = DataProtectionProvider.Create("YourApplicationName");
        
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContent, services) =>
            {
                services.AddScoped<ITokenService, TokenService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<ITokenStorage, TokenStorage>();
                services.AddScoped<IInformationService, InformationService>();
                services.AddScoped<LoginWindow>();
                services.AddScoped<RegistrationWindow>();
                services.AddScoped<DatabaseWindow>();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton<IDataProtectionProvider>(dataProtectionProvider);
                services.AddDbContext<UsersContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddDbContext<StorageContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issueAudience,
                            ValidAudience = issueAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issueSign!))
                        };
                    });
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
                SeedRoles(services.BuildServiceProvider()).Wait();
            })
            .Build();
    }
    
    private static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        var serviceProvider = AppHost.Services;

        var loginWindow = serviceProvider.GetService<LoginWindow>();
        loginWindow!.TokenService = serviceProvider.GetService<ITokenService>()!;
        loginWindow.UserService = serviceProvider.GetService<IUserService>()!;
        loginWindow.InformationService = serviceProvider.GetService<InformationService>()!;

        MainWindow = loginWindow;
        loginWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}