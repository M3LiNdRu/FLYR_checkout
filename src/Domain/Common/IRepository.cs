namespace Supermarket.Domain.Common;

public interface IRepository<T> where T : IAggregateRoot
{
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task<T> ReadAsync(string id, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
