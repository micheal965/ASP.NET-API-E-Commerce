using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using Talabat.Core.IServices;

namespace Talabat.APIs.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<IAuthService>();
            var token = context.Request.Headers["authorization"].FirstOrDefault()?.Replace("Bearer ", "").Trim();

            if (!string.IsNullOrEmpty(token) && await authService.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // Unauthorized
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("Token is invalid or expired.");
                return;
            }

            await _next(context);
        }
    }
}
