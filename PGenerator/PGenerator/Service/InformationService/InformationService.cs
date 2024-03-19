using PGenerator.Data;
using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;
using PGenerator.Service.PasswordService;

namespace PGenerator.Service.InformationService;

public class InformationService(StorageContext context, byte[] secretKey, byte[] iv) : IInformationService
{
    private StorageContext Context { get; set; } = context;
    public IList<Database> ListInformation(Guid userId)
    {
        var existingList = Context.Information.Where(information => information.UserId == userId).ToList();
        var newList = new List<Database>();
        for (var i = 0; i < existingList.Count; i++)
        {
            var newPassword = PasswordEncrypt.DecryptStringFromBytes_Aes(existingList[i].Password, secretKey, iv);
            var newElement = new Database(existingList[i].Id, existingList[i].Application!, existingList[i].UserName!, newPassword,
                existingList[i].Created);
            newList.Add(newElement);
        }

        return newList;
    }

    public async Task<Information> GetInformation(Guid infoId)
    {
        try
        {
            return Context.Information.FirstOrDefault(info => info.Id == infoId)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

    public async Task<PublicResponse> UpdateInfo(UpdateRequest request, Guid infoId)
    {
        Console.WriteLine(infoId);
        try
        {
            var existingInfo = Context.Information.FirstOrDefault(info => info.Id == infoId);
            if (existingInfo != null)
            {
                existingInfo.UserName = request.UserName;
                existingInfo.Application = request.Application;
                existingInfo.Password = PasswordEncrypt.EncryptStringToBytes_Aes(request.Password!, secretKey, iv);

                Context.Update(existingInfo);

                await Context.SaveChangesAsync();

                return new PublicResponse(true, string.Empty);
            }
            else
            {
                return new PublicResponse(false, "Cannot find info with this id.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PublicResponse> DeleteInfo(Guid infoId)
    {
        try
        {
            var existingInfo = Context.Information.FirstOrDefault(info => info.Id == infoId);
            Context.Information.Remove(existingInfo!);
            await Context.SaveChangesAsync();

            return new PublicResponse(true, string.Empty);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private bool CheckApplicationExist(string application)
    {
        return Context.Information.Any(info => info.Application == application);
    }
}