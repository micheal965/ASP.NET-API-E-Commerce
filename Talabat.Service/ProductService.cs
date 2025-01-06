using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.IServices;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product?>> GetProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecifications(productSpecParams);
            IReadOnlyList<Product> products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }
        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductSpecifications(productId);
            var Product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
            return Product;
        }
        public async Task<IReadOnlyList<Brand?>> GetBrandsAsync()
        {
            var Brands = await _unitOfWork.Repository<Brand>().GetAllAsync();
            return Brands;
        }

        public async Task<IReadOnlyList<Category?>> GetCategoriesAsync()
        {
            var Categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            return Categories;
        }

        public async Task<int> CountProductsAsync(ProductSpecifications ProductFilterationSpec)
        {
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(ProductFilterationSpec);
            return count;
        }
    }
}
