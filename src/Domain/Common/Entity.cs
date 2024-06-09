namespace Domain.Common;

public interface IEntity 
{
    string Id { get; }
}

public abstract class Entity : IEntity
{
    public string Id { get; protected set; }
}
