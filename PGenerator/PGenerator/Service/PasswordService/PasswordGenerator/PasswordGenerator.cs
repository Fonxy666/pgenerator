namespace PGenerator.Service.PasswordService.PasswordGenerator;

public static class PasswordGenerator
{
    public static string GeneratePassword()
    {
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string symbolChars = "!@#$%^&*()-_=+";
        const string numberChars = "0123456789";

        var allChars = lowercaseChars + uppercaseChars + symbolChars + numberChars;

        var rnd = new Random();
        var password = new char[12];

        password[0] = lowercaseChars[rnd.Next(lowercaseChars.Length)];
        password[1] = uppercaseChars[rnd.Next(uppercaseChars.Length)];
        password[2] = symbolChars[rnd.Next(symbolChars.Length)];
        password[3] = numberChars[rnd.Next(numberChars.Length)];

        for (var i = 4; i < 12; i++)
        {
            password[i] = allChars[rnd.Next(allChars.Length)];
        }

        for (var i = 0; i < password.Length; i++)
        {
            var randomIndex = rnd.Next(password.Length);
            (password[i], password[randomIndex]) = (password[randomIndex], password[i]);
        }

        return new string(password);
    }
}