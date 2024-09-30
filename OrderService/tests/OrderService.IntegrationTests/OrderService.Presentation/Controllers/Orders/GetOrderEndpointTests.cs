using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.BaseImplementations;
using OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers;
using OrderService.Presentation.Common.DTOs.OrderDTOs;
using OrderService.Application.Common.AppErrors;
using System.Net;
using System.Net.Http.Json;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders
{
    public class GetOrderEndpointTests
        : BaseOrderApiIntegrationTest
    {
        public GetOrderEndpointTests(OrderApiFactory apiFactory)
            : base(apiFactory)
        {
        }

        [Fact]
        public async Task GetOrderById_WithexistingId_ShouldReturnExistingOrder()
        {
            //Arrange
            var orderToGet = _orderGenerator.SeededOrders[0];
            var orderRequestToAssert = new OrderRequest(
                    orderToGet.CustomerId,
                    orderToGet.ProductQuantity
                );

            //Act
            var getResult =
                await _httpClient
                    .GetAsync(OrderApiUrl.GetOrderByIdEndpoint + orderToGet.Id);
            var orderResponse =
                await getResult.Content
                    .ReadFromJsonAsync<OrderResponse>();

            //Assert
            using var _ = new AssertionScope();

            getResult.StatusCode.AssertStatusCode(HttpStatusCode.OK);
            orderResponse!.AssertOrderResponse(orderRequestToAssert, orderToGet.OrderDate);
        }

        [Fact]
        public async Task GetOrderById_WithNonexistingId_ShouldReturnNotFoundError()
        {
            //Arrange
            var nonexistentId = Guid.NewGuid();
            var expectedError = Errors.Orders.NotFound(nonexistentId);

            //Act
            var getResult =
                await _httpClient
                    .GetAsync(OrderApiUrl.GetOrderByIdEndpoint + nonexistentId);
            var error =
                await getResult.Content
                    .ReadFromJsonAsync<ProblemDetails>();

            //Assert
            using var _ = new AssertionScope();

            getResult.StatusCode.AssertStatusCode(HttpStatusCode.NotFound);
            error!.AssertError(expectedError);
        }
    }
}