using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class NameEmailContactNumber_patch
    {
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format. Example: xyz@mail.com")]
        public string Email { get; set; }
       
        [Required]
        [MinLength(10, ErrorMessage = "Contact number must be at least 10 characters long.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid contact number")]
        public string? ContactNumber { get; set; }
    }
}
