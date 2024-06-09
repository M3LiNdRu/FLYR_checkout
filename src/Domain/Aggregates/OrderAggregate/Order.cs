using Supermarket.Domain.Common;

namespace Supermarket.Domain.Aggregates.OrderAggregate;

public class Order : Entity
{
    public Order(string storeId, string tillNumber, IEnumerable<OrderItem> items)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storeId, nameof(storeId));
        ArgumentException.ThrowIfNullOrWhiteSpace(tillNumber, nameof(tillNumber));

        Items = items;
        StoreId = storeId;
        TillNumber = tillNumber;
    }

    public IEnumerable<OrderItem> Items { get; }
    public string StoreId { get; }
    public string TillNumber { get; }
}
