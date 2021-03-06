using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Factory
{
    public interface IPaymentGatewayProvider
    {
        public bool Status { get; }
        public string PaymentGatewayName { get; }
        public string PaymentGatewayUrl { get; }
        public decimal MaxAmount { get; }
        public decimal MinimumAmount { get; }
        public Task<PostPaymentResult> ProcessPayment(PaymentRequest paymentRequest);
    }
}
