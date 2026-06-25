using Microsoft.EntityFrameworkCore;

using RabbitMQService.Application.Interfaces;
using RabbitMQService.Domain.Entities;
using RabbitMQService.Infrastructure.Data;

namespace RabbitMQService.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }


    public async Task<User?> UpdateAsync(
        Guid id, string? name, string? email, int? age,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null) return null;

        user.Update(name, email, age);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null) return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
    
}