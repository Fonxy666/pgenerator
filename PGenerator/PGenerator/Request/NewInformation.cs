using System.ComponentModel.DataAnnotations;

namespace PGenerator.Request;

public record NewInformation([Required]string UserId, [Required]string AppName, [Required]string UserName, [Required]string Password);