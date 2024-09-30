using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Entities;
using OrderService.Presentation.Common.DTOs.OrderDTOs;
using System.Net;

namespace OrderService.IntegrationTests.OrderService.Presentation.Controllers.Orders.TestUtuls.Helpers
{
    public static class AssertionExtensions
    {
        public static void AssertStatusCode(
                this HttpStatusCode responseStatusCode,
                HttpStatusCode expectedStatusCode
            )
        {
            responseStatusCode.Should().Be(expectedStatusCode);
        }

        public static void AssertOrderResponse(
                this OrderResponse orderResponse,
                OrderRequest expectedOrder
            )
        {
            orderResponse.CustomerId.Should().Be(expectedOrder.CustomerId);
            orderResponse.OrderDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            orderResponse.ProductQuantity.Should().Be(expectedOrder.ProductQuantity);
        }

        public static void AssertError(
                this ProblemDetails problemDetails,
                Error expectedError
            )
        {
            problemDetails.Status.Should().Be(expectedError.NumericType);
            problemDetails.Title.Should().Be(expectedError.Code);
            problemDetails.Detail.Should().Be(expectedError.Description);
        }
    }
}