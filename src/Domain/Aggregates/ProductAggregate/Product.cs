using Domain.Common;

namespace Domain.Aggregates.ProductAggregate;

public record Price(decimal Amount, string Currency);
public class Product : Entity
{
    public string Barcode { get; private set; }
    public string CodeName { get; private set; }
    public Price Price { get; private set; }

    public Product(string barcode, string codeName, Price price)
    {
        Id = Guid.NewGuid().ToString();
        Barcode = barcode;
        CodeName = codeName;
        Price = price;
    }
}