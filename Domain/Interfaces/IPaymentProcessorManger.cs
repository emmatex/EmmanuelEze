using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPaymentProcessorManger
    {
        public Task<PostPaymentResult> RoutePaymentRequest(PaymentRequest paymentRequest);
    }
}
