namespace Ecom.Core.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public string? Street { get; set; } 
        public string? City { get; set; } 
        public string? State { get; set; }
        public string? ZipCode { get; set; } 
    }
}
