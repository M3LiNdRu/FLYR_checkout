namespace Domain.Aggregates.OrderAggregate;

public interface IPricingRule 
{
    string Name { get; }
    string Barcode { get; }
    void Apply(OrderItem item);
}

public abstract class PricingRule : IPricingRule
{
    public PricingRule(string name, string barcode)
    {
        Name = name;
        Barcode = barcode;
    }

    public string Name { get; private set; }

    public string Barcode { get; private set; }

    public abstract void Apply(OrderItem item);
}
public class GetOneForFreePricingRule : PricingRule
{
    public GetOneForFreePricingRule(string name, string barcode) : base(name, barcode)
    {

    }

    public override void Apply(OrderItem item)
    {
        if (item.Product.Barcode != Barcode) 
            return;

        item.IncrementQuantity(1);
        item.ApplyDiscount(Name, item.Product.Price.Amount);
    }
}

public class BulkPurchasePricingRule : PricingRule
{
    public BulkPurchasePricingRule(string name, string barcode, 
        int minQuantity, decimal multiplier) : base(name, barcode)
    {
        MinimumQuantity = minQuantity;
        Multiplier = multiplier;
    }

    public int MinimumQuantity { get; private set; }
    public decimal Multiplier { get; private set; }

    public override void Apply(OrderItem item)
    {
        if (item.Product.Barcode != Barcode) 
            return;

        if (item.Quantity >= MinimumQuantity)
            item.ApplyDiscount(Name, Multiplier*item.Quantity);
    }
}