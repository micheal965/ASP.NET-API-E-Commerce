using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuggyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("InternalServerError")]
        public async Task<ActionResult> GetServerError()
        {
            var product = await _unitOfWork.Repository<Product>().GetAsync(-1);
            var producttoreturn = product.ToString();
            return Ok(producttoreturn);
        }

        [HttpGet("NotFound")]
        public async Task<ActionResult> GetNotFoundRequest() => NotFound();

        [HttpGet("BadRequest")]
        public async Task<ActionResult> GetBadRequest() => BadRequest();
        //Validation
        [HttpGet("BadRequest/{Id}")]
        public async Task<ActionResult> GetBadRequest(int Id) => Ok();

    }
}
