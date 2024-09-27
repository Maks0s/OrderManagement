namespace OrderService.Presentation.Common.DTOs.OrderDTOs
{
    public record OrderRequest(
            Guid CustomerId,
            int ProductQuantity
        );
}