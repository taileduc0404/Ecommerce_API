using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities
{
	public class Product : BaseEntity<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		//Navignational Property
		public virtual Category Category { get; set; }
		public int CategoyId { get; set; }
	}
}
