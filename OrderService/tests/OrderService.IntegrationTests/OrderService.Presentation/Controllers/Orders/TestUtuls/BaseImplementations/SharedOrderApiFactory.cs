namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.BaseImplementations
{
    [CollectionDefinition(Name)]
    public class SharedOrderApiFactory
        : ICollectionFixture<OrderApiFactory>
    {
        public const string Name = "SharedOrderApiFactory";
    }
}