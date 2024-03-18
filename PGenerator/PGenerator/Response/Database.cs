namespace PGenerator.Response;

public record Database(Guid MessageId, string Application, string Username, string Password, DateTime Created);