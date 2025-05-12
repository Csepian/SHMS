using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SHMS.DTO
{
    // data transfer objecct - client to server , only needed data contains , hide sensitive data , improve performance
    public class PaymentDTO
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public int BookingID { get; set; }
        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        public Decimal Amount { get; set; }
       
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        //status not beacause it validate payment and then add 
     
    }
}



