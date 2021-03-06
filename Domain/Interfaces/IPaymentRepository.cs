using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task AddPaymentState(Guid paymentId, PayState status);
        PaymentState GetPaymentState(Guid paymentId, Guid stateId);
        Task<Payment> GetByCardNo(string cardNo);
    }
}
