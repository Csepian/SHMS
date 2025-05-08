using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class BookingDTO
    {
        public int UserID { get; set; }
        [Required]
        public int RoomID { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]

        public DateTime CheckOutDate { get; set; }
    }
}
