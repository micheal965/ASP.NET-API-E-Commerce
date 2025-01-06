using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Core.IServices
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product?>> GetProductsAsync(ProductSpecParams productSpecParams);
        Task<Product?> GetProductAsync(int productId);
        Task<IReadOnlyList<Brand?>> GetBrandsAsync();
        Task<IReadOnlyList<Category?>> GetCategoriesAsync();

        Task<int> CountProductsAsync(ProductSpecifications ProductFilterationSpec);
    }
}
