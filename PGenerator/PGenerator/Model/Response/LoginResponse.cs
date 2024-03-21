namespace PGenerator.Model.Response;

public record LoginResponse(UserInformation? User, string? Message, bool Success);