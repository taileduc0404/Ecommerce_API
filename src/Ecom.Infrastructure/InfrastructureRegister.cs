using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Infrastructure
{
	public static class InfrastructureRegister
	{
		public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			//services.AddScoped<ICategoryRepository, CategoryRepository>();
			//services.AddScoped<IProductRepository, ProductRepository>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();


			//Congifure Db
			services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
			return services;
		}
	}
}
