using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public ICollection<PaymentState>  PaymentStates { get; set; }
            = new List<PaymentState>();
    }
}
