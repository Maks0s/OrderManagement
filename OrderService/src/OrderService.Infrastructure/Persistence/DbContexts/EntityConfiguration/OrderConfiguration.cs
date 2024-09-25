using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.DbContexts.EntityConfiguration
{
    public class OrderConfiguration
        : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(o => o.ProductQuantity)
                .IsRequired()
                .HasColumnType("int");
        }
    }
}
