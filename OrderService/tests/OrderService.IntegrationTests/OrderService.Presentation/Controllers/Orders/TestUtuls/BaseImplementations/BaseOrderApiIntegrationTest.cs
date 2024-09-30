using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.TestUtils;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.BaseImplementations
{
    [Collection(SharedOrderApiFactory.Name)]
    public class BaseOrderApiIntegrationTest
        : BaseApiIntegrationTest
    {
        protected OrderGenerator _orderGenerator;

        private readonly Func<Task> _asyncDbReseeder;

        public BaseOrderApiIntegrationTest(OrderApiFactory apiFactory) 
            : base(apiFactory)
        {
            _asyncDbReseeder = apiFactory.ReseedDbAsync;
            _orderGenerator = apiFactory.OrderGenerator;
        }

        public override async Task InitializeAsync()
        {
            await _asyncDbReseeder.Invoke();
        }
    }
}
