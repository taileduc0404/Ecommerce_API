using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Data.Config;
using Ecom.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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

			//Congifure Identity
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMemoryCache();
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			});

			return services;
		}

		public static async void InfrastructureConfigMiddleware(IApplicationBuilder builder)
		{
			using (var scope = builder.ApplicationServices.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				await IdentityConfiguration.SeedUserAsync(userManager);
			}
		}
	}
}
