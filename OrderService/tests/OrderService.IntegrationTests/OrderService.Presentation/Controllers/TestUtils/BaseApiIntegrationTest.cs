using MassTransit.Testing;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.TestUtils
{
    public class BaseApiIntegrationTest : IAsyncLifetime
    {
        protected readonly Func<Task> _asyncDbReseter;
        protected readonly HttpClient _httpClient;
        protected readonly ITestHarness _testHarness;

        public BaseApiIntegrationTest(
                BaseApiFactory apiFactory
            )
        {
            _asyncDbReseter = apiFactory.ResetDbAsync;
            _httpClient = apiFactory.HttpClient;
            _testHarness = apiFactory.TestHarness;
        }

        public virtual async Task InitializeAsync()
        {
            await _asyncDbReseter.Invoke();
        }

        public virtual async Task DisposeAsync() => await Task.CompletedTask;
    }
}