using Supermarket.Domain.Aggregates.OrderAggregate;
using Supermarket.Domain.Aggregates.ProductAggregate;
using Microsoft.Extensions.Logging;
using Supermarket.Console;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("Supermarket", LogLevel.Information)
        .AddConsole();
});
var logger = loggerFactory.CreateLogger<PointOfSale>();
var pointOfSale = new PointOfSale(logger, "S0001", "T01");

logger.LogInformation("Small Supermarkets - Point of Sale");

//Store Products
var tea = new Product("GR1", "Green tea", new Price(3.11M, "GBP"));
var strawberries = new Product("SR1", "Strawberries", new Price(5.0M, "GBP"));
var coffee = new Product("CF1", "Coffee", new Price(11.23M, "GBP"));

pointOfSale.SetStoreInventory([tea, strawberries, coffee]);

//Define Pricing Rules
var ceo_rule = new GetOneForFreePricingRule("buy-one-get-one-for-free", tea.Barcode);
var coo_rule = new BulkPurchasePricingRule(
    name: "bulk-purchase-x3+-offer", 
    barcode: strawberries.Barcode, 
    minQuantity: 3, 
    multiplier: 0.5M);
var cto_rule = new BulkPurchasePricingRule(
    name: "special-offer", 
    barcode: coffee.Barcode, 
    minQuantity: 3, 
    multiplier: Math.Round(coffee.Price.Amount * (1.0M / 3.0M), 2));

pointOfSale.LoadPricingRule(ceo_rule);
pointOfSale.LoadPricingRule(coo_rule);
pointOfSale.LoadPricingRule(cto_rule);

logger.LogInformation("Scan any product for checkout.");

pointOfSale.Scan(tea);
pointOfSale.Scan(strawberries);
pointOfSale.Scan(strawberries);
pointOfSale.Scan(strawberries);
pointOfSale.Scan(strawberries);
pointOfSale.Scan(coffee);
pointOfSale.Scan(coffee);
pointOfSale.Scan(coffee);

pointOfSale.ProceedCheckout();

logger.LogInformation("Thanks for you purchase :)");


