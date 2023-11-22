using System.ComponentModel.DataAnnotations;

namespace BootcampAPI.Models.Dto
{
    public class OrderHeaderUpdateDto
    {
        public int OrderHeaderId { get;set; }
        public string PickupName { get; set; }
        public string PickupPhoneNumber { get; set; }
        public string PickupEmail { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
    }
}
