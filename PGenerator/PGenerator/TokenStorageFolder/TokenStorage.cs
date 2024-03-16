using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace PGenerator.TokenStorageFolder
{
    public class TokenStorage : ITokenStorage
    {
        private readonly string _tokenFilePath = Path.Combine(Directory.GetCurrentDirectory(), "token_storage", "token.txt");
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public TokenStorage(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;

            Directory.CreateDirectory(Path.GetDirectoryName(_tokenFilePath)!);
        }

        public async Task SaveTokenAsync(string token)
        {
            var protector = _dataProtectionProvider.CreateProtector("JwtToken");
            var encryptedToken = protector.Protect(token);

            await File.WriteAllTextAsync(_tokenFilePath, encryptedToken);
        }

        public async Task<string?> ReadTokenAsync()
        {
            if (!File.Exists(_tokenFilePath))
            {
                return null;
            }

            var encryptedToken = await File.ReadAllTextAsync(_tokenFilePath);

            var protector = _dataProtectionProvider.CreateProtector("JwtToken");
            return protector.Unprotect(encryptedToken);
        }
    }
}