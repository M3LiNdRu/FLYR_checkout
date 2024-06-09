using AutoFixture;
using Supermarket.Domain.Aggregates.OrderAggregate;
using FluentAssertions;

namespace Supermarket.Domain.Tests.Aggregates.OrderAggregate
{
    public class GetOneForFreePricingRuleTest
    {
        private readonly Fixture _fixture;

        public GetOneForFreePricingRuleTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Apply_WhenBarcodeDoesNotMatch_EnsuresValidState()
        {
            //Arrange
            var item = _fixture.Create<OrderItem>();
            var sut = new GetOneForFreePricingRule("buy-one-get-one-for-free", "barcode-test");

            //Act
            sut.Apply(item);

            //Assert
            item.Quantity.Should().Be(1);
            item.Discount.Should().BeNull();
        }

        [Fact]
        public void Apply_WhenBarcodeMatches_EnsuresValidState()
        {
            //Arrange
            var item = _fixture.Create<OrderItem>();
            var sut = new GetOneForFreePricingRule("buy-one-get-one-for-free", item.Product.Barcode);

            //Act
            sut.Apply(item);

            //Assert
            item.Quantity.Should().Be(2);
            item.Discount.Should().NotBeNull();
            item.Discount.Price.Amount.Should().Be(item.Product.Price.Amount * -1);
        }
    }
}
