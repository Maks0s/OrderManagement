namespace Shared.Contracts
{
    public record OrderCreatedEvent
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductQuantity { get; set; }
    }
}