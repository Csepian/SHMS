using System.ComponentModel.DataAnnotations;

namespace SHMS.DTOs
{
    public class HotelDTO
    {
        public int HotelID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Location { get; set; }

        [StringLength(200)]
        public string? Amenities { get; set; }

        public double? Rating { get; set; }
    }
}
