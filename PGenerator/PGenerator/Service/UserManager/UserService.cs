using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PGenerator.Response;
using Microsoft.AspNetCore.Identity;
using PGenerator.Model;
using PGenerator.Request;

namespace PGenerator.Service.UserManager;

public class UserService(UserManager<UserInformation> userManager) : IUserService
{
    public async Task<IList<UserInformation>> ListUsers()
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
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                var errorMessages = validationResults.Select(vr => vr.ErrorMessage);
                foreach (var errorMessage in errorMessages)
                {
                    switch (errorMessage)
                    {
                        case "The field Password must be a string or array type with a minimum length of '8'.":
                            return new PublicResponse(false, "Password length minimum 8.");
                        
                        case @"The field Password must match the regular expression '^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$'.":
                            return new PublicResponse(false, "Password should contain number & symbol & uppercase letters.");
                        
                        case "The Email field is not a valid e-mail address.":
                            return new PublicResponse(false, "Not a valid e-mail address.");
                    }
                }
            }
            
            if (userManager.Users.Any(user => user.Email == request.Email))
            {
                return new PublicResponse(false, "Email already in use.");
            }
            
            if (userManager.Users.Any(user => user.UserName == request.UserName))
            {
                return new PublicResponse(false, "Username already in use.");
            }
            
            var user = new UserInformation
            {
                UserName = request.UserName,
                Email = request.Email
            };

            await userManager.CreateAsync(user, request.Password!);
            return new PublicResponse(true, "Registration successful.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            
            if (existingUser == null)
            {
                return new LoginResponse(null, "Invalid username or password.", false);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(existingUser!, request.Password);
            if (!isPasswordValid)
            {
                return new LoginResponse(null, "Invalid username or password.", false);
            }

            return new LoginResponse(existingUser, "", true);
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