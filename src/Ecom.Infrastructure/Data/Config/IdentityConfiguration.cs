using Ecom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
	public class IdentityConfiguration
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				// create new user
				var user = new ApplicationUser
				{
					DisplayName = "Duc Tai",
					Email = "tai996507@gmail.com",
					UserName = "tai996507@gmail.com",
					Address = new Address
					{
						FirstName = "Tai",
						LastName = "Le Duc",
						City = "Ho Chi Minh",
						State = "Binh Tan",
						Street = "31/28 Nguyen Quy Yem",
						ZipCode = "123"
					}
				};

				await userManager.CreateAsync(user, "P@ssword1");
				
			}
		}
	}
}
