using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace SHMS.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Payment>? Payments { get; set; }
//Navigation Property
        public Hotel? Hotel { get; set; }

        
    }
}
