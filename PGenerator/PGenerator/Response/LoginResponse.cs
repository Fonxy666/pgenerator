using PGenerator.Model;

namespace PGenerator.Response;

public record LoginResponse(UserInformation? User, string? Message, bool Success);