using Domain.Entities.Enums;
using System;

namespace Domain.Entities
{
    public class PaymentState : BaseEntity
    {
        public PayState Status { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
