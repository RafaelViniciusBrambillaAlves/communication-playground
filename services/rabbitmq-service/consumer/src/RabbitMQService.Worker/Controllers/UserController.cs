using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQService.Application.DTOs;
using RabbitMQService.Application.UseCases.GetAllUsers;
using RabbitMQService.Application.UseCases.GetUserById;
using RabbitMQService.Domain.Entities;

namespace RabbitMQService.Worker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class UsersController(
    GetAllUsersUseCase getAllUsers,
    GetUserByIdUseCase getUSerById
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await getAllUsers.ExecuteAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await getUSerById.ExecuteAsync(id, cancellationToken);
        return Ok(user);
    }
}