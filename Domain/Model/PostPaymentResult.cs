using Domain.Entities.Enums;
using System;

namespace Domain.Model
{
    public class PostPaymentResult
    {
        public string TransactionRef { get; set; }
        public PayState PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string ProviderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentGatewayUrl { get; set; }
    }
}
