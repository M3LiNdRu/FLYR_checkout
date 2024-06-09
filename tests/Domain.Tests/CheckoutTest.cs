using Domain.Aggregates.OrderAggregate;
using Domain.Aggregates.ProductAggregate;
using FluentAssertions;
using AutoFixture;

namespace Domain.Tests;

public class CheckoutTest
{
    private readonly Fixture _fixture;

    public CheckoutTest()
    {
        _fixture = new Fixture();    
    }

    [Fact]
    public void Ctor_Initialize_EnsuresValidState()
    {
        //Arrange
        IEnumerable<PricingRule> rules = null;

        //Act
        var act = () => new Checkout(rules);

        //Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Scan_WhenProductNotExistInItems_ProductIsAdded()
    {
        //Arrange
        var sut = new Checkout([]);
        var product = _fixture.Create<Product>();

        //Act
        sut.Scan(product);

        //Assert
        sut.Items.Should().ContainSingle(i => i.Product == product);
    }

    [Fact]
    public void Scan_WhenProductExistInItems_QuantityIsIncremented()
    {
        //Arrange
        var sut = new Checkout([]);
        var product = _fixture.Create<Product>();
        sut.Scan(product);

        //Act
        sut.Scan(product);

        //Assert
        sut.Items.Should().ContainSingle(i => i.Product == product);
        sut.Items.Single().Quantity.Should().Be(2);
    }

    [Fact]
    public void Total_WhenGetOneFreePricingRuleApplied_EnsuresValidState()
    {
        //Arrange
        var product = _fixture.Create<Product>();
        var rule = new GetOneForFreePricingRule("buy-one-get-one-for-free", product.Barcode);
        var sut = new Checkout([rule]); 
        sut.Scan(product);

        //Act
        var result = sut.Total();

        //Assert
        sut.Items.Single().Quantity.Should().Be(2);
        result.Amount.Should().Be(product.Price.Amount);
        result.Currency.Should().Be(product.Price.Currency);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(9)]
    public void Total_WhenBulkPurchasePricingRuleApplied_EnsuresValidState(int quantity)
    {
        //Arrange
        var product = _fixture.Create<Product>();
        var multiplier = 0.5M;
        var rule = new BulkPurchasePricingRule("bulk-purchase-x3+-discount", product.Barcode, quantity, multiplier);
        var sut = new Checkout([rule]); 
        SetupMultipleScans(quantity, () => sut.Scan(product));
        var expectedCurrency = product.Price.Currency;
        var expectedAmount = product.Price.Amount*quantity - multiplier*quantity;

        //Act
        var result = sut.Total();

        //Assert
        sut.Items.Single().Quantity.Should().Be(quantity);
        result.Amount.Should().Be(expectedAmount);
        result.Currency.Should().Be(expectedCurrency);
    }

    private void SetupMultipleScans(int times, Action action) 
    {
        foreach (var i in Enumerable.Range(0, times)) 
        {
            action();
        }
    }
}