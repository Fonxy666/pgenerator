using PGenerator.Model;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public interface IInformationService
{
    IList<Database> ListInformation(Guid userId);
    Task<PublicResponse> AddNewInfo(Information request);
    Task<PublicResponse> UpdatePassword(Information request, string infoId);
    Task<PublicResponse> DeleteInfo(Guid messageId);
}