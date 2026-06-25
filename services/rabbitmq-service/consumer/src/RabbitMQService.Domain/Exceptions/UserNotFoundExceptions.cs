namespace RabbitMQService.Domain.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id) 
        : base($"User with id '{id}' was not found") { }
}