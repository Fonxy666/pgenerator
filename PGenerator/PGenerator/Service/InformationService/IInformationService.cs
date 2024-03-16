using PGenerator.Model;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public interface IInformationService
{
    Task<IList<Information>> ListInformation(string userId);
    Task<PublicResponse> AddNewInfo(Information request);
    Task<PublicResponse> UpdatePassword(Information request, string infoId);
    Task<PublicResponse> DeleteInfo(string userId, string oldPassword, string newPassword);
}