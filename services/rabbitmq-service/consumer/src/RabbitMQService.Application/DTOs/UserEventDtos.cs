namespace RabbitMQService.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string Name,
    string Email,
    int Age, 
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record UserEventMessage(
    Guid EventId,
    string EventType,
    DateTime Timestamp,
    System.Text.Json.JsonElement Payload
);

public sealed record UserCreatedPayload(
    Guid UserId,
    string Name,
    string Email,
    int Age
);

public sealed record UserUpdatedPayload(
    Guid UserId,
    string? Name, 
    string? Email,
    int? Age
);

public sealed record UserDeletedPayload(
    Guid UserId
);
