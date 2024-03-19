using PGenerator.Data;
using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;
using PGenerator.Service.PasswordService;

namespace PGenerator.Service.InformationService;

public class AccountDetailService(AccountStorageContext context, byte[] secretKey, byte[] iv) : IAccountDetailService
{
    private AccountStorageContext Context { get; set; } = context;
    public IList<AccountDetailShow> ListInformation(Guid userId)
    {
        var existingList = Context.AccountDetails.Where(information => information.UserId == userId).ToList();
        var newList = new List<AccountDetailShow>();
        for (var i = 0; i < existingList.Count; i++)
        {
            var newPassword = PasswordEncrypt.DecryptStringFromBytes_Aes(existingList[i].Password, secretKey, iv);
            var newElement = new AccountDetailShow(existingList[i].Id, existingList[i].Application!, existingList[i].UserName!, newPassword,
                existingList[i].Created);
            newList.Add(newElement);
        }

        return newList;
    }

    public async Task<AccountDetail> GetInformation(Guid infoId)
    {
        try
        {
            return Context.AccountDetails.FirstOrDefault(info => info.Id == infoId)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PublicResponse> AddNewInfo(AccountDetail request)
    {
        try
        {
            if (!CheckApplicationExist(request.Application!))
            {
                await Context.AccountDetails.AddAsync(request);
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
            var existingInfo = Context.AccountDetails.FirstOrDefault(info => info.Id == infoId);
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
            var existingInfo = Context.AccountDetails.FirstOrDefault(info => info.Id == infoId);
            Context.AccountDetails.Remove(existingInfo!);
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
        return Context.AccountDetails.Any(info => info.Application == application);
    }
}