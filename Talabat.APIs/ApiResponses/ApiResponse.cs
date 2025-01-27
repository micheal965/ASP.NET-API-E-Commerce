

using System.Net;

namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetMessageForStatusCode(StatusCode);
        }

        private string? GetMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status500InternalServerError => "Internal Server Error"
            };
        }
    }
}
