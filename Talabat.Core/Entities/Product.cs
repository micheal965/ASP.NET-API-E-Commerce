namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        //foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        //foreign key
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
