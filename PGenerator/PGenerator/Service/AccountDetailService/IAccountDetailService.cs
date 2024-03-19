using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public interface IAccountDetailService
{
    IList<AccountDetailShow> ListInformation(Guid userId);
    Task<AccountDetail> GetInformation(Guid infoId);
    Task<PublicResponse> AddNewInfo(AccountDetail request);
    Task<PublicResponse> UpdateInfo(UpdateRequest request, Guid infoId);
    Task<PublicResponse> DeleteInfo(Guid infoId);
}