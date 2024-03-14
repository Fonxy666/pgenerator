using System.ComponentModel.DataAnnotations;

namespace PGenerator.Request;

public class RegistrationRequest(string? userName, string? password, string? email)
{
    [Required]public string? UserName { get; set; } = userName;
    [Required, MinLength(8), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")]public string? Password { get; set; } = password;
    [Required, EmailAddress]public string? Email { get; set; } = email;
}