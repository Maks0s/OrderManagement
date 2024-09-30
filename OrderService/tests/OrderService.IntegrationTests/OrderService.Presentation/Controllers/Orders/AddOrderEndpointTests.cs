using FluentAssertions.Execution;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.BaseImplementations;
using System.Net.Http.Json;
using System.Net;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers;
using OrderService.Presentation.Common.DTOs.OrderDTOs;
using FluentAssertions;
using Shared.Contracts;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders
{
    public class AddOrderEndpointTests
        : BaseOrderApiIntegrationTest
    {
        public AddOrderEndpointTests(OrderApiFactory apiFactory)
            : base(apiFactory)
        {
        }

        [Fact]
        public async Task AddOrder_WithValidData_ShouldReturnCreatedOrder()
        {
            //Arrange
            var orderToAdd = _orderGenerator.GenerateOrderRequest();

            //Act
            var addResult =
                await _httpClient
                    .PostAsJsonAsync(OrderApiUrl.AddOrderEndpoint, orderToAdd);

            var addedOrder =
                await addResult.Content
                    .ReadFromJsonAsync<OrderResponse>();

            //Assert
            using var _ = new AssertionScope();

            var publishingResult = await _testHarness.Published.Any<OrderCreatedEvent>();
            publishingResult.Should().BeTrue();
            addResult.StatusCode.AssertStatusCode(HttpStatusCode.Created);
            addResult.Headers.Location.Should().Be($"http://localhost/orders/{addedOrder!.Id}");
            addedOrder!.AssertNewlyCreatedOrderResponse(orderToAdd);
        }

        [Fact]
        public async Task AddOrder_WithInvalidData_ShouldReturnValidationError()
        {
            //Arrange
            var invalidOrderToAdd = _orderGenerator.GenerateInvalidOrderRequest();

            //Act
            var addResult =
                await _httpClient
                    .PostAsJsonAsync(OrderApiUrl.AddOrderEndpoint, invalidOrderToAdd);

            //Assert
            using var _ = new AssertionScope();

            addResult.StatusCode.AssertStatusCode(HttpStatusCode.BadRequest);
        }
    }
}