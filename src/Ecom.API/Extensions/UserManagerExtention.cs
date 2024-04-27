using Ecom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.API.Extensions
{
	public static class UserManagerExtention
	{
		// Tìm người dùng theo yêu cầu chính là ĐỊA CHỈ
		public static async Task<ApplicationUser> FindUserByClaimPrincipalWithAddress(this UserManager<ApplicationUser> userManager,
							ClaimsPrincipal user)
		{
			var email = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
		}
		public static async Task<ApplicationUser> FindEmailByClaimPrincipal(this UserManager<ApplicationUser> userManager,
							ClaimsPrincipal user)
		{
			var email = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
		}
	}
}
