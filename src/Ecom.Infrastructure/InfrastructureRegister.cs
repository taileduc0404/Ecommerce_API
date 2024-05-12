using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Data.Config;
using Ecom.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

			// Configure token services
			services.AddScoped<ITokenService, TokenService>();


			//Congifure Db
			services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			//Congifure Identity
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMemoryCache();

			//Congifure JWT Token
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			})
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
							ValidIssuer = configuration["Token:Issuer"],
							ValidateIssuer = true,
							ValidateAudience = false
						};
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
