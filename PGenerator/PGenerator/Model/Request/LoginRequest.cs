using System.ComponentModel.DataAnnotations;

namespace PGenerator.Model.Request;

public record LoginRequest([Required]string UserName, [Required]string Password);