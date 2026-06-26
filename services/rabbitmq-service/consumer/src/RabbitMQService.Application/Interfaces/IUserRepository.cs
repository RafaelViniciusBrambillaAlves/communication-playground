using RabbitMQService.Domain.Entities;

namespace RabbitMQService.Application.Interfaces;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default); 

    Task<User?> UpdateAsync(Guid id, string? name, string? email, int? age,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);
}