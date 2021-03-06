using Domain.Factory;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.PaymentProvider
{
    public class PaymentProcessorManager : IPaymentProcessorManger
    {
        private readonly IServiceProvider _serviceProvider;
        public PaymentProcessorManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<PostPaymentResult> RoutePaymentRequest(PaymentRequest paymentRequest)
        {
            var paymentResult = new PostPaymentResult();
            var paymentgateways = _serviceProvider.GetRequiredService<IEnumerable<IPaymentGatewayProvider>>().ToList();
            // filter the payment gateway that can handle this request based on the requiremnet
            var paymentGateway = paymentgateways.FirstOrDefault(pg => pg.Status && pg.MinimumAmount <= paymentRequest.Amount && pg.MaxAmount >= paymentRequest.Amount);
            if (paymentGateway == null && paymentRequest.Amount > 20 && paymentRequest.Amount <= 500)
            {
                //Use IExpensivePaymentGateway if available. Otherwise, retry only once with ICheapPaymentGateway.
                var cheapPaymentGateway = _serviceProvider.GetRequiredService<ICheapPaymentGateway>();
                paymentResult = await cheapPaymentGateway.ProcessPayment(paymentRequest);
                return paymentResult;
            }
            else
            {
                paymentResult = await paymentGateway.ProcessPayment(paymentRequest);
                return paymentResult;
            }
        }
    }
}

