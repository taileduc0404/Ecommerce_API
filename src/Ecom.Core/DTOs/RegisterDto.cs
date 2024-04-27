using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.DTOs
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress(ErrorMessage = "Mày phải nhập email đúng định dạng")]
		public string Email { get; set; }
		[Required]
		[MinLength(3, ErrorMessage = "Độ dài nhỏ nhất của email là 3 ký tự nha 'bitch'")]
		public string DisplayName { get; set; }
		[Required]
		[RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
			ErrorMessage = "Ít nhất 1 chữ cái viết thường, 1 chữ in hoa, 1 chữ số, 1 ký tự đặc biệt và độ dài từ 6-10 ký tự. Thứ tự các ký tự không quan trọng.")]
		public string Password { get; set; }
	}
}
