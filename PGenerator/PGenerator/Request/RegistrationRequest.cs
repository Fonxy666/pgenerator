using System.ComponentModel.DataAnnotations;

namespace PGenerator.Request;

public record RegistrationRequest(
    [Required]string UserName,
    [Required, MinLength(8), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")]string Password,
    [Required, EmailAddress]string Email
    );