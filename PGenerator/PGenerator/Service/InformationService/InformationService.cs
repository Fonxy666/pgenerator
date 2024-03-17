using PGenerator.Data;
using PGenerator.Model;
using PGenerator.Response;

namespace PGenerator.Service.InformationService;

public class InformationService(StorageContext context) : IInformationService
{
    private StorageContext Context { get; set; } = context;
    public async Task<IList<Information>> ListInformation(Guid userId)
    {
        return Context.Information.Where(information => information.UserId == userId).ToList();
    }

    public async Task<PublicResponse> AddNewInfo(Information request)
    {
        try
        {
            if (!CheckApplicationExist(request.Application!))
            {
                await Context.Information.AddAsync(request);
                await Context.SaveChangesAsync();
                return new PublicResponse(true, string.Empty);
            }
            else
            {
                return new PublicResponse(false, "Application is already in the database.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<PublicResponse> UpdatePassword(Information request, string infoId)
    {
        throw new NotImplementedException();
    }

    public Task<PublicResponse> DeleteInfo(string userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    private bool CheckApplicationExist(string application)
    {
        return Context.Information.Any(info => info.Application == application);
    }
}