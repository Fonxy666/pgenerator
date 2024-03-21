using System.ComponentModel.DataAnnotations;

namespace PGenerator.Model.Request;

public class UpdateRequest(string application, string userName, string password)
{
    [Required]public string? Application { get; set; } = application;
    [Required]public string? UserName { get; set; } = userName;
    [Required, MinLength(8), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")]public string? Password { get; set; } = password;
}