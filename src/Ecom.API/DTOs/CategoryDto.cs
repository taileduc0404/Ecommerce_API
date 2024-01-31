using System.ComponentModel.DataAnnotations;

namespace Ecom.API.DTOs
{
	public class CategoryDto
	{
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
