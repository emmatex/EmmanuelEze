using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(DataContext context) : base(context)
        {
        }

        public async Task AddPaymentState(Guid paymentId, PayState status)
        {
            if (paymentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(paymentId));
            }

            var state = new PaymentState
            {
                PaymentId = paymentId,
                Id = Guid.NewGuid(),
                Status = status
            };
            await _context.PaymentStates.AddAsync(state);
        }

        public async Task<Payment> GetByCardNo(string cardNo)
        {
            return await _context.Payments.SingleOrDefaultAsync(x => x.CreditCardNumber == cardNo);
        }

        public override async Task<Payment> GetByIdAsync(Guid id)
        {
            return await _context.Payments.Include(x => x.PaymentStates)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public PaymentState GetPaymentState(Guid paymentId, Guid stateId)
        {
            if (paymentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(paymentId));
            }

            if (stateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(stateId));
            }

            return _context.PaymentStates.SingleOrDefault(c => c.PaymentId == paymentId && c.Id == stateId);
        }
    }
}
