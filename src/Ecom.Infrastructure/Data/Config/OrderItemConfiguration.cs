using Ecom.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Config
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.OwnsOne(x => x.ProductItemOrdered, n => { n.WithOwner(); });
			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
		}
	}
}
