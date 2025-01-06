namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductSpecParams
    {
        //pagination
        private int pagesize = 5;
        private int MaxpageSize = 10;
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxpageSize ? MaxpageSize : value; }
        }

        public int pageindex { get; set; } = 1;
        //orderby
        public string? sort { get; set; }
        //filteration by brand and category
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        //search filteration

        private string? searchname;

        public string? SearchName
        {
            get { return searchname; }
            set { searchname = value?.ToLower(); }
        }


    }
}
