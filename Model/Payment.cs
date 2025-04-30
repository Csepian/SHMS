using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHMS.Model
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int BookingID { get; set; }
        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        public Decimal Amount { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        //Navigation Properties
        [ForeignKey("UserID")]
        public User? User { get; set; }
        [ForeignKey("BookingID")]
        public Booking? Booking { get; set; }
    }
}
