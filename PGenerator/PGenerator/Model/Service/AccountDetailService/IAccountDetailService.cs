using PGenerator.Model.Request;
using PGenerator.Model.Response;

namespace PGenerator.Model.Service.AccountDetailService;

public interface IAccountDetailService
{
    IList<AccountDetailShow> ListInformation(Guid userId);
    Task<AccountDetail> GetInformation(Guid infoId);
    Task<PublicResponse> AddNewInfo(AccountDetail request);
    Task<PublicResponse> UpdateInfo(UpdateRequest request, Guid infoId);
    Task<PublicResponse> DeleteInfo(Guid infoId);
}