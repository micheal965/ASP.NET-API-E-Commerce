using AutoMapper;
using Talabat.APIs.DTOs.Product;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlReslover : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;


        public ProductPictureUrlReslover(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {

                return Path.Combine(_configuration["HttpsApi"], source.PictureUrl);
            }
            return "";
        }
    }
}
