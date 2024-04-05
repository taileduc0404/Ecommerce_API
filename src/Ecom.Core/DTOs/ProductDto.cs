using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.DTOs
{
    public class BaseProduct
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 9999, ErrorMessage = "Price Limited By {0} and {1}")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} Must Be A Number!")]
        public decimal Price { get; set; }
    }
    public class ProductDto : BaseProduct
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }
    }

    public class ReturnProductDto
    {
        public int TotalItems { get; set; }
    }
    public class AddProductDto : BaseProduct
    {
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }

    }
    public class UpdateProductDto : BaseProduct
    {
        public int CategoryId { get; set; }
        public string OldImage { get; set; }
        public IFormFile Image { get; set; }
    }
}
