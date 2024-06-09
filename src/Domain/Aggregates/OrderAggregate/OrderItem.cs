using Supermarket.Domain.Aggregates.ProductAggregate;

namespace Supermarket.Domain.Aggregates.OrderAggregate;

public record Discount(string Description, Price Price);

public class OrderItem
{
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
        var invertAmount = amount * -1;
        Discount = new Discount(description, new Price(invertAmount, Product.Price.Currency));
    }
}
