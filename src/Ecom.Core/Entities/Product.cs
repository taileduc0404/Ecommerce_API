
namespace Ecom.Core.Entities
{
	public class Product : BaseEntity<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string ProductPicture { get; set; }

		//Navignational Property
		public int CategoryId { get; set; }
		public virtual Category Category { get; set; }
	}
}
