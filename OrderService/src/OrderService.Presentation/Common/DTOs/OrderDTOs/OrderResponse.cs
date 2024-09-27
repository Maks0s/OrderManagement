namespace OrderService.Presentation.Common.DTOs.OrderDTOs
{
    public record OrderResponse(
            Guid Id,
            Guid CustomerId,
            DateTime OrderDate,
            int ProductQuantity
        );
}