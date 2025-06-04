using System.ComponentModel.DataAnnotations;

namespace SHMS.DTOs
{
    public class HotelDTO
    {

        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [Required]
        [StringLength(500)]
        public string? Location { get; set; }
        [Required]
        public int? ManagerID { get; set; }

        [StringLength(200)]
        public string? Amenities { get; set; }

    }
}
