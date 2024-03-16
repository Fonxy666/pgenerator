using System.ComponentModel.DataAnnotations;

namespace PGenerator.Request;

public record LoginRequest([Required]string UserName, [Required]string Password);