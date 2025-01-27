using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.IRepositories;
using Talabat.Core.IServices;
using Talabat.Repository;
using Talabat.Service;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            //caching
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    //key value 
                    //value.errors
                    //errors.errormessage
                    //key value 
                    //key value 

                    var errors = actioncontext.ModelState.Where(p => p.Value.Errors.Count > 0)
                                             .SelectMany(p => p.Value.Errors)
                                             .Select(p => p.ErrorMessage).ToArray();

                    var apiresponse = new ApiValidationErrorsResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(apiresponse);
                };
            });


        }
    }
}
