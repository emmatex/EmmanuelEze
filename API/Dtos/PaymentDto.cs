using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PaymentDto
    {
        [Required]
        [MaxLength(16, ErrorMessage = "Credit card number shouldn't have more than 16 characters.")]
        public string CreditCardNumber { get; set; }
        [Required]
        [MaxLength(256, ErrorMessage = "Card holder shouldn't have more than 256 characters.")]
        public string CardHolder { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Security code shouldn't have more than 3 characters.")]
        public string SecurityCode { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }
    }
}
