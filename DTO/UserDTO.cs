using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format. Example: xyz@mail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Password must contain letters and numbers and special characters.")]

        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Contact number must be at least 10 characters long.")]
        public string? ContactNumber { get; set; }
    }
}