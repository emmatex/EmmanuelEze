using Domain.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IPaymentRepository Payment { get; }
        public UnitOfWork(DataContext context, IPaymentRepository payment)
        {
            _context = context;
            Payment = payment;
        }

       

        public async Task<int> Complete() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();

    }
}
