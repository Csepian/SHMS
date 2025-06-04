using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format. Example: xyz@mail.com")]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
    }
}
// 2af9a6c642aaaac7f7e68d1ec8acaed0d9eee757c9084b22594d61cc69bccd9d

