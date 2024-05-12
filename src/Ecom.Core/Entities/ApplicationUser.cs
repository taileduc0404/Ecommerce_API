using Microsoft.AspNetCore.Identity;

namespace Ecom.Core.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string DisplayName { get; set; }
		public Address Address { get; set; }
	}
}
