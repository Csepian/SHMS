using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace SHMS.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format. Example: xyz@mail.com")]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Password must contain letters, numbers and special characters.")]

        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Contact number must be at least 10 characters long.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid contact number")]
        public string? ContactNumber { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Payment>? Payments { get; set; }
//Navigation Property
        public Hotel? Hotel { get; set; }
        public string SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
            return Password;
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
    }
}

        
   