namespace NotificationService.Infrastructure.RabbitMq.Common
{
    public class RabbitMqConfig
    {
        public const string SectionName = "RabbitMqConfig";
        public string Host { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
