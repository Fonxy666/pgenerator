using DatabaseLibrary.Model;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public class InformationService : IInformationService
{
    public Task<IList<Information>> ListInformation(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<PublicResponse> AddNewInfo(Information request)
    {
        throw new NotImplementedException();
    }

    public Task<PublicResponse> UpdatePassword(Information request, string infoId)
    {
        throw new NotImplementedException();
    }

    public Task<PublicResponse> DeleteInfo(string userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }
}