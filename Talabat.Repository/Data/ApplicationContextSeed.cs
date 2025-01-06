using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class ApplicationContextSeed
    {
        public async static Task DataSeed(ApplicationDbContext _dbcontext)
        {
            //seeding Brands
            var BrandsJson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
            var Brands = JsonSerializer.Deserialize<List<Brand>>(BrandsJson);
            if (_dbcontext.Brands.Count() == 0)
            {
                if (Brands?.Count > 0)
                {
                    await _dbcontext.Set<Brand>().AddRangeAsync(Brands);
                }
            }

            //seeding Caregories
            var CaregoryJson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(CaregoryJson);
            if (_dbcontext.Categories.Count() == 0)
            {
                if (categories?.Count > 0)
                {
                    await _dbcontext.Set<Category>().AddRangeAsync(categories);
                }
            }

            //seeding Products
            var ProductJson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
            var Products = JsonSerializer.Deserialize<List<Product>>(ProductJson);
            if (_dbcontext.Products.Count() == 0)
            {
                if (Products?.Count > 0)
                {
                    await _dbcontext.Set<Product>().AddRangeAsync(Products);
                }
            }

            //Seeding Deliverymethods
            var DeliverymethodsJson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
            var Deliverymethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliverymethodsJson);
            if (_dbcontext.DeliveryMethods.Count() == 0)
            {
                if (Deliverymethods?.Count > 0)
                {
                    await _dbcontext.Set<DeliveryMethod>().AddRangeAsync(Deliverymethods);
                }
            }
            await _dbcontext.SaveChangesAsync();
        }
    }
}
