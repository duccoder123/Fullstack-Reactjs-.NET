using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootcampAPI.Models.Dto
{
    public class OrderHeaderCreateDto
    {
        [Required]
        public string PickupName { get; set; }
        [Required]
        public string PickupPhoneNumber { get; set; }
        [Required]

        public string PickupEmail { get; set; }
        public string ApplicationUserId { get; set; }
        public double OrderTotal { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<OrderDetailsCreateDto> OrderDetailsDTO { get; set; }
    }
}
