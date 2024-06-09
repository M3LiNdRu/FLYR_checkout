namespace Domain.Common;

public interface IEntity 
{
    string Id { get; }
}

public abstract class Entity : IEntity
{
    public required string Id { get; init; }
}
