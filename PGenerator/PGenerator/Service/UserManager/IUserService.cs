using DatabaseLibrary.Model;
using PGenerator.Request;
using PGenerator.Response;

namespace PGenerator.Service.UserManager;

public interface IUserService
{
    Task<IList<User>> ListUsers();
    Task<UserResponse> Registration(RegistrationRequest request);
    Task<UserResponse> Login(LoginRequest request);
    Task<UserResponse> ChangePassword(string userId, string oldPassword, string newPassword);
    Task<UserResponse> DeleteUser(string userId);
}