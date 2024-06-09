using Domain.Aggregates.ProductAggregate;
using Domain.Common;

namespace Domain.Aggregates.OrderAggregate;

public record Discount(string Description, Price Price);

public class OrderItem {
    public OrderItem(Product product, int quantity, Discount discount) : this(product)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantity, 0, nameof(quantity));
        ArgumentNullException.ThrowIfNull(discount);
        Quantity = quantity;
        Discount = discount;
    }

    public OrderItem(Product product)
    {
        ArgumentNullException.ThrowIfNull(product); 
        Product = product;
        Quantity = 1;
    }

    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public Discount? Discount { get; private set; }

    public void IncrementQuantity(int inc)
    {
        Quantity += inc;
    }

    public Price CalculatePrice() 
    {
        var discount = Discount is not null ? Discount.Price.Amount : 0;
        var amount = (Product.Price.Amount * Quantity) + discount;
        return new Price(amount, Product.Price.Currency);
    }

    public void ApplyDiscount(string description, decimal amount)
    {
        var invertAmount = amount*-1;
        Discount = new Discount(description, new Price(invertAmount, Product.Price.Currency));
    }
}

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

        applyPricingRules();

        var currency = _items.First().Product.Price.Currency;
        var totalAmount = _items.Sum(i => i.CalculatePrice().Amount);
        return new Price(totalAmount, currency);
    }

    private void applyPricingRules() 
    {
        foreach (var item in _items) {
            foreach (var rule in Rules) {
                rule.Apply(item);
            }
        }
    }
}