using GrpcService.Application.Dtos;
using GrpcService.Domain.Entities;

namespace GrpcService.Application.Mappers;

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