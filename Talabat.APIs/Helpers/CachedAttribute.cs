using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using Talabat.Core.IServices;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timetolive;

        public CachedAttribute(int timetolive)
        {
            _timetolive = timetolive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var Cacheservice = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cachekey = await GenerateCacheKey(context.HttpContext.Request);

            var CachedResponse = await Cacheservice.GetCachedResponseAsync(cachekey);

            //if cached
            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var contentresult = new ContentResult()
                {
                    Content = CachedResponse,
                    StatusCode = (int)HttpStatusCode.OK,
                    ContentType = "application/json"

                };
                context.Result = contentresult;
                return;
            }

            //if not cached
            var executedendpoint = await next();// will execute the end
            if (executedendpoint.Result is OkObjectResult okObjectResult)
                await Cacheservice.CacheResponseAsync(cachekey, executedendpoint.Result, TimeSpan.FromSeconds(_timetolive));

        }
        private async Task<string> GenerateCacheKey(HttpRequest request)
        {

            //url/api/Product?CategoryId=1&brandId=1
            //url/path/attributes
            var keybuilder = new StringBuilder();
            keybuilder.Append(request.Path);//api/product
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keybuilder.Append($"|{key}-{value}");
            }
            return keybuilder.ToString();
        }
    }
}
