using API.Controllers;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PaymentControllerUnitTest
    {
        [Fact]
        public async Task Process_Payment_ThenItShouldReturnsBadRequest()
        {
            //Arrange
            var mockUoW = new Mock<IUnitOfWork>();
            var mockPpm = new Mock<IPaymentProcessorManger>();
            var _ctr = new PaymentsController(mockUoW.Object, mockPpm.Object);

            //Act
            var contentResult = await _ctr.ProcessPayment(new PaymentRequest
            {
                Amount = 20,
                CardHolder = "John Doe",
                CreditCardNumber = "333333333333333333",
                SecurityCode = "613",
                ExpirationDate = DateTime.Now
            });

            //Assert
            Assert.IsType<BadRequestObjectResult>(contentResult);         
        }

    }
}
