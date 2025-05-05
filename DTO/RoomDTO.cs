using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    public class RoomDTO
    {
        public int HotelID { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        public Decimal Price { get; set; }
        [Required]
        public bool Availability { get; set; }
        [StringLength(500)]
        public string? Features { get; set; }
    }
}
