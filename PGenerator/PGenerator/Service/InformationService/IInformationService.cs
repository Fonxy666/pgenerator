using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public interface IInformationService
{
    IList<Database> ListInformation(Guid userId);
    Task<AccountInformation> GetInformation(Guid infoId);
    Task<PublicResponse> AddNewInfo(AccountInformation request);
    Task<PublicResponse> UpdateInfo(UpdateRequest request, Guid infoId);
    Task<PublicResponse> DeleteInfo(Guid infoId);
}