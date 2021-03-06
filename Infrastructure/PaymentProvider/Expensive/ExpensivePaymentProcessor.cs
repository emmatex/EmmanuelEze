using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.PaymentProvider.Expensive
{
    public class ExpensivePaymentProcessor : IExpensivePaymentGateway
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ExpensivePaymentProcessor(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Status => true;
        public string PaymentGatewayName => "Expensive Payment Gateway";
        public decimal MaxAmount => 500;
        public decimal MinimumAmount => 21;
        public string PaymentGatewayUrl => "https://expensivepayment.com";

        public async Task<PostPaymentResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            var paymentProcessorStatus = RemotePaymentGatewayStatus.Process();
            var paymentData = _mapper.Map<Payment>(paymentRequest);
            var paymentFromDb = await _unitOfWork.Payment.GetByCardNo(paymentData.CreditCardNumber);
            if (paymentFromDb != null)
            {
                await _unitOfWork.Payment.AddPaymentState(paymentFromDb.Id, paymentProcessorStatus);
            }
            else
            {
                paymentData.Id = Guid.NewGuid();
                await _unitOfWork.Payment.AddPaymentState(paymentData.Id, paymentProcessorStatus);
                await _unitOfWork.Payment.Add(paymentData);
            }
            return new PostPaymentResult
            {
                Amount = paymentData.Amount,
                CreatedDate = paymentData.CreatedOn.Date,
                ProviderName = PaymentGatewayName,
                PaymentStatus = paymentProcessorStatus,
                PaymentGatewayUrl = PaymentGatewayUrl,
                TransactionRef = "E-REF-02"
            };
        }

    }
}
