namespace DatabaseLibrary.Model;

public class Storage(Guid userId, string? application, string? userName, string? password)
{
    public Guid UserId { get; set; } = userId;
    public string? Application { get; set; } = application;
    public string? UserName { get; set; } = userName;
    public string? Password { get; set; } = password;
    public DateTime Created { get; set; } = DateTime.Now;
}