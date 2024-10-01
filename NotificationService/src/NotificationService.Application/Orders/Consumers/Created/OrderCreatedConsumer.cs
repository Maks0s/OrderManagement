using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace NotificationService.Application.Orders.Consumers.Created
{
    public class OrderCreatedConsumer
        : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation(
                    "Received {@eventName}: {@eventDetails}; Sending email with order information to: test@gmail.com",
                    nameof(context.Message),
                    context.Message
                );

            return Task.CompletedTask;
        }
    }
}