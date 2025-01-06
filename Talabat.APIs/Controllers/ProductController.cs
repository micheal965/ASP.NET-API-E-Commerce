using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.Product;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.IServices;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public IMapper Mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            Mapper = mapper;
        }
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var products = await _productService.GetProductsAsync(productSpecParams);
            if (products == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            //count data after filteration210
            var ProductFilterationSpec = new ProductSpecifications(productSpecParams.BrandId,
                                                                   productSpecParams.CategoryId,
                                                                   productSpecParams.SearchName);

            int count = await _productService.CountProductsAsync(ProductFilterationSpec);

            var data = Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            //pageindex
            //pagesize
            //count
            //data
            return Ok(new Pagination<ProductToReturnDto>(productSpecParams.pageindex, productSpecParams.PageSize, count, data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{Id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int Id)
        {
            var Product = await _productService.GetProductAsync(Id);
            if (Product == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok(Mapper.Map<Product, ProductToReturnDto>(Product));
        }

        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("GetBrands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            var Brands = await _productService.GetBrandsAsync();
            if (Brands == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok(Brands);
        }

        [HttpGet("GetCategories")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCaregories()
        {
            var Categories = await _productService.GetCategoriesAsync();
            if (Categories == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok(Categories);
        }
    }
}
