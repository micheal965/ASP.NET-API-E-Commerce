using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs.Order;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost("Checkout")]
        [ProducesResponseType<Order>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> CreateAsync(OrderDto orderDto)
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var shippingaddress = _mapper.Map<AddressDto, Address>(orderDto.shippingaddress);
            var Order = await _orderService.CreateOrderAsync(buyeremail, orderDto.BasketId, orderDto.DeliveryMethodId, shippingaddress);
            if (Order == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            return Ok(orderDto);
        }

        [HttpGet("GetAllOrdersForUser")]
        [ProducesResponseType<Order>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            var ordertoreturnDto = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            if (orders == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
            return Ok(ordertoreturnDto);
        }
        [HttpGet("GetOrderForUser")]
        [ProducesResponseType<Order>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int orderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(orderId, email);
            var orderToReturnDto = _mapper.Map<Order, OrderToReturnDto>(order);
            if (orderToReturnDto == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
            return Ok(orderToReturnDto);
        }

        [HttpGet("GetDeliveryMethods")]
        [ProducesResponseType<DeliveryMethod>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod?>>> GetDeliveryMethods()
        {
            var delivermethods = await _orderService.GetDeliveryMethods();
            if (delivermethods == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
            return Ok(delivermethods);
        }
    }
}
