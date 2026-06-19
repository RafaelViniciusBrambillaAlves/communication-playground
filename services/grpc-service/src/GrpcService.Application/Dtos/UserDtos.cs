namespace GrpcService.Application.Dtos;

public sealed record UserDto(
    Guid Id,
    string Name,
    string Email,
    int Age,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record CreateUseDto(
    string Name, 
    string Email,
    int Age
);

public sealed record UpdateUserDto(
    Guid Id,
    string? Name,
    string? Email,
    int? Age
);
