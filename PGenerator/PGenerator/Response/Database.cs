namespace PGenerator.Response;

public record Database(Guid InfoId, string Application, string Username, string Password, DateTime Created);