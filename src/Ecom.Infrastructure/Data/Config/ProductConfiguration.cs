using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{

		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(x => x.Id).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
		}
	}
}
