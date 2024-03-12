using DatabaseLibrary.Model;
using Microsoft.EntityFrameworkCore;
using PGenerator.Response;
using Microsoft.AspNetCore.Identity;
using PGenerator.Request;

namespace PGenerator.Service.UserManager;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<IList<User>> ListUsers()
    {
        try
        {
            return await userManager.Users.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserResponse> Registration(RegistrationRequest request)
    {
        try
        {
            if (userManager.Users.Any(user => user.Email == request.Email))
            {
                return new UserResponse(false, "Email already in use.");
            }
            
            if (userManager.Users.Any(user => user.UserName == request.UserName))
            {
                return new UserResponse(false, "Username already in use.");
            }
            
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            await userManager.CreateAsync(user, request.Password);
            return new UserResponse(true, "Registration successful.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserResponse> Login(LoginRequest request)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            
            if (existingUser == null)
            {
                return new UserResponse(false, "Invalid username or password.");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(existingUser!, request.Password);
            if (!isPasswordValid)
            {
                return new UserResponse(false, "Wrong password.");
            }

            return new UserResponse(true, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserResponse> ChangePassword(string userId, string oldPassword, string newPassword)
    {
        try
        {
            var existingUser = await userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return new UserResponse(false, "User not exist");
            }

            await userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
            
            return new UserResponse(true, "Successful update.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserResponse> DeleteUser(string userId)
    {
        try
        {
            var existingUser = await userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return new UserResponse(false, "User not exist");
            }

            await userManager.DeleteAsync(existingUser);
            
            return new UserResponse(true, "Successful delete.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}