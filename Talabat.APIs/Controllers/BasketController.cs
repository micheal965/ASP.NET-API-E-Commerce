using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.Errors;
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


        //Get existing one (with data or empty after expiring time)
        [HttpGet]//basket?Id=
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string Id)
        {
            CustomerBasket? Basket = await basketRepository.GetBasketAsync(Id);
            return Ok(Basket ?? new CustomerBasket(Id));
        }

        //Create for the first time or Update 
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasket)
        {
            CustomerBasket Basket = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasket);

            CustomerBasket? UpdatedOrCreatedBasket = await basketRepository.UpdateBasketAsync(Basket);

            if (UpdatedOrCreatedBasket is null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            return Ok(UpdatedOrCreatedBasket);
        }

        [HttpDelete]//Basket?Id=
        public async Task<ActionResult<bool>> DeleteBasket(string Id)
        {
            return await basketRepository.DeleteBasketAsync(Id);
        }
    }
}
