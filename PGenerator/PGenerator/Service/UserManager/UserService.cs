using Microsoft.EntityFrameworkCore;
using PGenerator.Response;
using Microsoft.AspNetCore.Identity;
using PGenerator.Model;
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

    public async Task<PublicResponse> Registration(RegistrationRequest request)
    {
        try
        {
            if (userManager.Users.Any(user => user.Email == request.Email))
            {
                return new PublicResponse(false, "Email already in use.");
            }
            
            if (userManager.Users.Any(user => user.UserName == request.UserName))
            {
                return new PublicResponse(false, "Username already in use.");
            }
            
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            await userManager.CreateAsync(user, request.Password);
            return new PublicResponse(true, "Registration successful.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PublicResponse> Login(LoginRequest request)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            
            if (existingUser == null)
            {
                return new PublicResponse(false, "Invalid username or password.");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(existingUser!, request.Password);
            if (!isPasswordValid)
            {
                return new PublicResponse(false, "Wrong password.");
            }

            return new PublicResponse(true, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PublicResponse> ChangePassword(string userId, string oldPassword, string newPassword)
    {
        try
        {
            var existingUser = await userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return new PublicResponse(false, "User not exist");
            }

            await userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
            
            return new PublicResponse(true, "Successful update.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PublicResponse> DeleteUser(string userId)
    {
        try
        {
            var existingUser = await userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return new PublicResponse(false, "User not exist");
            }

            await userManager.DeleteAsync(existingUser);
            
            return new PublicResponse(true, "Successful delete.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}