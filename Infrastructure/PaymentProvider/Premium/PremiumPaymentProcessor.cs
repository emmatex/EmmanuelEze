using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Model;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.PaymentProvider.Premium
{
    public class PremiumPaymentProcessor : IPremiumPaymentGateway
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PremiumPaymentProcessor(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Status => true;
        public string PaymentGatewayName => "Premium Payment Gateway";
        public decimal MaxAmount => 1000;
        public decimal MinimumAmount => 501;
        public string PaymentGatewayUrl => "https://premiumpayment.com";


        private int _retryCount = 0;

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
            await Retry(paymentRequest, paymentProcessorStatus, _retryCount);
            return new PostPaymentResult
            {
                Amount = paymentData.Amount,
                CreatedDate = paymentData.CreatedOn.Date,
                ProviderName = PaymentGatewayName,
                PaymentStatus = paymentProcessorStatus,
                PaymentGatewayUrl = PaymentGatewayUrl,
                TransactionRef = "P-REF-03"
            };
        }

        private async Task Retry(PaymentRequest paymentRequest, PayState state, int count)
        {
            if (state != PayState.Processed && count <= 3)
            {
                _retryCount++;
                await ProcessPayment(paymentRequest);
            }
        }

    }
}
