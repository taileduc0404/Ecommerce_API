using Ecom.Core.Entities;
using Ecom.Core.Entities.Orders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecom.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public virtual DbSet<Product> products { get; set; }
		public virtual DbSet<Category> categories { get; set; }
		public virtual DbSet<Address> addresses { get; set; }
		public virtual DbSet<Order> orders { get; set; }
		public virtual DbSet<OrderItem> orderItems { get; set; }
		public virtual DbSet<DeliveryMethod> deliveryMethods { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
