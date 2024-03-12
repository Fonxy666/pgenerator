using DatabaseLibrary.Model;
using PGenerator.Request;
using PGenerator.Response;

namespace PGenerator.Service.UserManager;

public interface IUserService
{
    Task<IList<User>> ListUsers();
    Task<PublicResponse> Registration(RegistrationRequest request);
    Task<PublicResponse> Login(LoginRequest request);
    Task<PublicResponse> ChangePassword(string userId, string oldPassword, string newPassword);
    Task<PublicResponse> DeleteUser(string userId);
}