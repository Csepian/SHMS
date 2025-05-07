using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHMS.Model
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int RoomID { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public string? Status { get; set; } = "Unconfirmed"; //Default value 

        //Navigation Properties
        [ForeignKey("UserID")]
        public User? User { get; set; }
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }
        public Payment? Payment { get; set; }

    }
}
