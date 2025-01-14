using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("CreateOrUpdatePaymentIntent/{BasketId}")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(BasketId);

            if (basket == null) return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "There is an Error with your Basket"));

            return Ok(basket);
        }

    }
}
