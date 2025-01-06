namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statuscode, string message, string? details = null) : base(statuscode, message)
        {
            Details = details;
        }
    }
}
