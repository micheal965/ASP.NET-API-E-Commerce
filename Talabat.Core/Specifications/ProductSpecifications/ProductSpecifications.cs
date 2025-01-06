
using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductSpecifications : BaseSpecifications<Product>
    {
        //For returning data
        public ProductSpecifications(ProductSpecParams productSpecParams)
            : base(p =>
            (productSpecParams.BrandId.Equals(null) || p.BrandId == productSpecParams.BrandId) &&
            (productSpecParams.CategoryId.Equals(null) || p.CategoryId == productSpecParams.CategoryId) &&
             string.IsNullOrEmpty(productSpecParams.SearchName) || p.Name.ToLower().Contains(productSpecParams.SearchName))
        {
            //ِAdd Includes
            AddIncludes();

            //Add Orders ex:Asc Desc
            AddOrders(productSpecParams.sort);
            //50 Product
            //10 Pages
            //5 product
            ApplyPagination(((productSpecParams.pageindex - 1) * productSpecParams.PageSize), productSpecParams.PageSize);
        }
        //For counting returning data after filteration=>where
        public ProductSpecifications(int? BrandId, int? CategoryId, string? SearchName) :
            base(p =>
                  (BrandId.Equals(null) || p.BrandId == BrandId) &&
                   (CategoryId.Equals(null) || p.CategoryId == CategoryId) &&
                    string.IsNullOrEmpty(SearchName) || p.Name.ToLower().Contains(SearchName))
        {

        }
        public ProductSpecifications(int Id)
            : base(p => p.Id == Id)
        {
            AddIncludes();

        }

        private void AddIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
        private void AddOrders(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "PriceAsc":
                        AddOrderAsc(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderDesc(p => p.Price);
                        break;
                    default:
                        AddOrderAsc(p => p.Name);
                        break;

                }
            }
            else
            {
                AddOrderAsc(p => p.Name);
            }
        }
        private void ApplyPagination(int skip, int take)
        {
            IsEnabledPagination = true;
            Skip = skip;
            Take = take;
        }

    }
}
