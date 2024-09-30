using Bogus;
using OrderService.Domain.Entities;
using OrderService.IntegrationTests.TestUtils;
using OrderService.Presentation.Common.DTOs.OrderDTOs;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers
{
    public class OrderGenerator
    {
        private readonly Faker<Order> _orderGenerator =
            new Faker<Order>()
                .WithRecord()
                .RuleFor(o => o.CustomerId, f => Guid.NewGuid())
                .RuleFor(o => o.OrderDate, f => f.Date.Recent(2))
                .RuleFor(o => o.ProductQuantity, f => f.Random.Int(1, 9999));

        private readonly Faker<OrderRequest> _orderRequestGenerator =
            new Faker<OrderRequest>()
                .WithRecord()
                .RuleFor(o => o.CustomerId, f => Guid.NewGuid())
                .RuleFor(o => o.ProductQuantity, f => f.Random.Int(1, 9999));


        private readonly Faker<OrderRequest> _invalidOrderGenerator =
            new Faker<OrderRequest>()
                .WithRecord()
                .RuleFor(o => o.CustomerId, f => Guid.NewGuid())
                .RuleFor(o => o.ProductQuantity, f => f.Random.Int(-9999, 0));


        public List<Order> SeededOrders;

        public OrderGenerator(int productCountToSeed)
        {
            SeededOrders = _orderGenerator.Generate(productCountToSeed);
        }

        public OrderRequest GenerateOrderRequest()
        {
            return _orderRequestGenerator.Generate();
        }

        public OrderRequest GenerateInvalidOrderRequest()
        {
            return _invalidOrderGenerator.Generate();
        }
    }
}