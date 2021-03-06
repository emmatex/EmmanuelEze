using API.Errors;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentProcessorManger _processorManger;

        public PaymentsController(IUnitOfWork unitOfWork, IPaymentProcessorManger processorManger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _processorManger = processorManger ?? throw new ArgumentNullException(nameof(processorManger));
        }

        [HttpPost(Name = "ProcessPayment")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ProcessPayment(PaymentRequest dto)
        {
            if ((dto.ExpirationDate - DateTime.Today) <= TimeSpan.Zero)
                return BadRequest(new ApiResponse(400, $"Credit card has expired"));

            if (!IsCreditCardInfoValid(dto.CreditCardNumber, dto.ExpirationDate.ToString("MM/yyyy"), dto.SecurityCode))
                return BadRequest(new ApiResponse(400, $"Invalid card"));

            var paymentResult = await _processorManger.RoutePaymentRequest(dto);
            if (await _unitOfWork.Complete() <= 0)
                return StatusCode(500, new ApiResponse(500, $"An error occured while trying to process payment"));

            if (paymentResult.PaymentStatus != PayState.Processed)
                return BadRequest(paymentResult);

            return Ok(paymentResult);
        }

        private static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
        {
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
                return false;
            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                return false;

            //NOTE: the date is base on the server date setting
            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy       
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }


    }
}
