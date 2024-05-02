using Ecom.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Config
{
	public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
	{
		public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
		{
			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
			builder.HasData(
				new DeliveryMethod { Id = 1, ShortName = "DHL", Description = "Fastest Delivery Time", Price = 20 },
				new DeliveryMethod { Id = 2, ShortName = "John", Description = "Get it with 3 days", Price = 10 },
				new DeliveryMethod { Id = 3, ShortName = "Laura", Description = "Slower but cheap", Price = 5 },
				new DeliveryMethod { Id = 4, ShortName = "Hitler", Description = "Free", Price = 0 },
				new DeliveryMethod { Id = 5, ShortName = "Tom", Description = "Not Free", Price = 100 }
				);
		}
	}
}
