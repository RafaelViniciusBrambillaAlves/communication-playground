using RabbitMQService.Application.DTOs;
using RabbitMQService.Domain.Entities;

namespace RabbitMQService.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Name,
        user.Email,
        user.Age,
        user.CreatedAt,
        user.UpdatedAt
    );
}