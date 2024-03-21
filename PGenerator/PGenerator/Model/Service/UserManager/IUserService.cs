using PGenerator.Model.Request;
using PGenerator.Model.Response;

namespace PGenerator.Model.Service.UserManager;

public interface IUserService
{
    Task<IList<UserInformation>> ListUsers();
    Task<PublicResponse> Registration(RegistrationRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task<PublicResponse> ChangePassword(string userId, string oldPassword, string newPassword);
    Task<PublicResponse> DeleteUser(string userId);
}