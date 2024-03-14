using System.ComponentModel.DataAnnotations;

namespace PGenerator.Model;

public class Information(Guid userId, string? application, string? userName, string? password)
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; } = userId;
    public string? Application { get; set; } = application;
    public string? UserName { get; set; } = userName;
    public string? Password { get; set; } = password;
    public DateTime Created { get; set; } = DateTime.Now;
}