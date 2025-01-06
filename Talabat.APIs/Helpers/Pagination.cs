using Talabat.APIs.DTOs.Product;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class Pagination<T> where T : ProductToReturnDto
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public IReadOnlyList<T> data { get; set; }
        public Pagination(int pageindex, int pagesize, int count, IReadOnlyList<T> data)
        {
            pageIndex = pageindex;
            pageSize = pagesize;
            this.count = count;
            this.data = data;
        }
        public Pagination(int pageindex, int pagesize, IReadOnlyList<T> data)
        {
            pageIndex = pageindex;
            pageSize = pagesize;
            this.data = data;
        }
    }
}