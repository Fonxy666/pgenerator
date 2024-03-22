using System.IO;
using System.Security.Cryptography;
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
using PGenerator.Data.TokenStorageFolder;
using PGenerator.Model;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.AuthService;
using PGenerator.Model.Service.UserManager;
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
        var secretKey = configuration["SecretKey"]!;
        var keyAsByte = Array.ConvertAll(secretKey.Split(','), byte.Parse);
        var ivKey = configuration["Iv"]!;
        var ivAsByte = Array.ConvertAll(ivKey.Split(','), byte.Parse);
        
        var dataProtectionProvider = DataProtectionProvider.Create("YourApplicationName");
        
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContent, services) =>
            {
                services.AddScoped<ITokenService, TokenService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<ITokenStorage, TokenStorage>();
                services.AddScoped<IAccountDetailService, AccountDetailService>(serviceProvider =>
                {
                    var storageContext = serviceProvider.GetRequiredService<AccountStorageContext>();
                    return new AccountDetailService(storageContext, keyAsByte, ivAsByte);
                });
                services.AddScoped<LoginWindow>(serviceProvider =>
                {
                    var userService = serviceProvider.GetRequiredService<IUserService>();
                    var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                    var tokenStorage = serviceProvider.GetRequiredService<ITokenStorage>();
                    var informationService = serviceProvider.GetRequiredService<IAccountDetailService>();
                    return new LoginWindow(userService, tokenService, tokenStorage, informationService, keyAsByte, ivAsByte);
                });
                services.AddScoped<RegistrationWindow>();
                services.AddScoped<DatabaseWindow>();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton<IDataProtectionProvider>(dataProtectionProvider);
                services.AddDbContext<UsersContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddDbContext<AccountStorageContext>(options =>
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
                SeedRoles(services.BuildServiceProvider(), configuration).Wait();
            })
            .Build();
    }
    
    private static async Task SeedRoles(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var adminEmail = configuration["AdminEmail"];
        var adminUserName = configuration["AdminUsername"];
        var adminPassword = configuration["AdminPassword"];
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserInformation>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail!);
        if (adminUser == null)
        {
            adminUser = new UserInformation { UserName = adminUserName!, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword!);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        var serviceProvider = AppHost.Services;
        
        var loginWindow = serviceProvider.GetService<LoginWindow>();
        MainWindow = loginWindow;
        loginWindow!.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}