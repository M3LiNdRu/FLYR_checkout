using Supermarket.Domain.Aggregates.OrderAggregate;
using Supermarket.Domain.Aggregates.ProductAggregate;
using Microsoft.Extensions.Logging;

namespace Supermarket.Console
{
    public class PointOfSale
    {
        private readonly ILogger<PointOfSale> _logger;
        private readonly List<Product> _products;
        private readonly List<IPricingRule> _pricingRules;
        private readonly Checkout _checkout;

        public PointOfSale(ILogger<PointOfSale> logger, string storeId, string tillNumber) 
        {
            _logger = logger;
            _products = new List<Product>();
            _pricingRules = new List<IPricingRule>();
            _checkout = new Checkout(_pricingRules);

            _logger.LogInformation("Till number {n} has been activated for store {s}", tillNumber, storeId);
        }

        public void SetStoreInventory(IEnumerable<Product> products)
        {
            _products.AddRange(products);

            _logger.LogInformation("{p} new products has been configured.", products.Count());
        }

        public void LoadPricingRule(IPricingRule rule)
        {
            _pricingRules.Add(rule);

            _logger.LogInformation("New {t} pricing rule with name {n} has been configured", rule.GetType(), rule.Name);
        }

        public void Scan(Product p)
        {
            _checkout.Scan(p);

            _logger.LogInformation("Product {p} has been scanned properly.", p.CodeName);
        }

        public void ProceedCheckout()
        {
            var totalPrice = _checkout.Total();

            _logger.LogInformation("Total Items: {items}", _checkout.Items.Count());
            _logger.LogInformation("Total Price: {v} {c}", totalPrice.Amount, totalPrice.Currency);
        }
    }
}
