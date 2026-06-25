namespace RabbitMQService.Domain.Entities;

public abstract class EntityBase
{
    protected EntityBase(): this(Guid.NewGuid()) { }

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}