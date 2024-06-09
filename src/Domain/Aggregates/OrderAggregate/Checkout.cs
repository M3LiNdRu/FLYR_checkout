using Supermarket.Domain.Aggregates.ProductAggregate;
using Supermarket.Domain.Common;

namespace Supermarket.Domain.Aggregates.OrderAggregate;

public class Checkout : IAggregateRoot
{
    private const string DefaultCurrency = "GBP";

    public Checkout(IEnumerable<IPricingRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        Rules = rules;
        _items = new List<OrderItem>();
    }

    public IEnumerable<IPricingRule> Rules { get; }

    private readonly List<OrderItem> _items;
    public IEnumerable<OrderItem> Items => _items;

    public void Scan(Product item) 
    {
        var orderItem = _items.SingleOrDefault(i => i.Product.Barcode == item.Barcode);
        if (orderItem is not null)
        {
            orderItem.IncrementQuantity(1);
        }
        else
        {
            _items.Add(new OrderItem(item));
        }

        //TODO: Raise domain event
    }

    public Price Total() 
    {
        if (!_items.Any())
            return new Price(0, DefaultCurrency);

        ApplyPricingRules();

        var currency = _items.First().Product.Price.Currency;
        var totalAmount = _items.Sum(i => i.CalculatePrice().Amount);
        return new Price(totalAmount, currency);
    }

    public void CreateOrder()
    {
        //TODO: Place an order
    }

    private void ApplyPricingRules() 
    {
        foreach (var item in _items) {
            foreach (var rule in Rules) {
                rule.Apply(item);
            }
        }
    }
}