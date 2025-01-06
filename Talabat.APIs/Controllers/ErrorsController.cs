using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("Errors/{statuscode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int statuscode)
        {
            return NotFound(new ApiResponse(statuscode));
        }
    }
}

