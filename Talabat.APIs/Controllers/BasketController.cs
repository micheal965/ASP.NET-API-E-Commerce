using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("GetBasketById")]//basket?Id=
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string Id)
        {
            CustomerBasket? Basket = await basketRepository.GetBasketAsync(Id);
            return Ok(Basket ?? new CustomerBasket(Id));
        }

        //Create for the first time or Update 
        [HttpPost("CreateOrUpdateBasket")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto customerBasket)
        {
            CustomerBasket Basket = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasket);

            CustomerBasket? UpdatedOrCreatedBasket = await basketRepository.UpdateBasketAsync(Basket);

            if (UpdatedOrCreatedBasket is null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            return Ok(UpdatedOrCreatedBasket);
        }

        [HttpDelete("DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Basket ID cannot be null or empty."));

            bool result = await basketRepository.DeleteBasketAsync(id);

            if (result)
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Basket deleted successfully."));
            else
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Basket not found."));
        }

    }
}
