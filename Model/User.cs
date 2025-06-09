using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        [NotMapped]
        public ICollection<Booking>? Bookings { get; set; }
        [NotMapped]

        public ICollection<Review>? Reviews { get; set; }
        [NotMapped]
        public ICollection<Payment>? Payments { get; set; }
        //Navigation Property
        [NotMapped]
        public Hotel? Hotel { get; set; }

        
    }
}
