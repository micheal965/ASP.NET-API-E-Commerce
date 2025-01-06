using System.Net;
namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorsResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorsResponse() : base(StatusCodes.Status400BadRequest)
        {
            Errors = new List<string>();
        }


    }
}
