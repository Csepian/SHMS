using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class ReviewDTO
    {
        public int UserID { get; set; }
        [Required]
        public int HotelID { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [StringLength(1000)]
        public string? Comment { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
