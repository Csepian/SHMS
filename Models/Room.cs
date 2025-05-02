using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace SHMS.Model
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        [Required]
        public int HotelID { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [Column(TypeName ="Decimal(18,2)")]
        public Decimal Price { get; set; }
        [Required]
        public bool Availability { get; set; }
        [StringLength(500)]
        public string? Features { get; set; }
//Navigation Properties
        [ForeignKey("HotelID")]
        public Hotel? Hotel { get; set; }
    }
}
