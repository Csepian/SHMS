using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SHMS.Model
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int HotelID { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [StringLength(1000)]
        public string? Comment { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    //Navigation Properties
        [ForeignKey("UserID")]
        public User? User { get; set; }
        [ForeignKey("HotelID")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Hotel? Hotel { get; set; } 

    }
}
