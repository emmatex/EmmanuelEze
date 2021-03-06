using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payment { get; }
        Task<int> Complete();
    }
}
