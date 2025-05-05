using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SHMS.Model
{
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Location { get; set; }
        [Required]
        public int? ManagerID { get; set; }
        [StringLength(200)]
        public string? Amenities { get; set; }
        public double? Rating { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Review> Reviews { get; set; }
        
        //Navigation Property
        [ForeignKey("ManagerID")]
        public User? Manager { get; set; }
    }
}
