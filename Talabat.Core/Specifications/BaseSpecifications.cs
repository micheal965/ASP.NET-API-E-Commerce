using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderByAsc { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsEnabledPagination { get; set; }

        public BaseSpecifications()
        {
            //Criteria =null

        }
        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        protected void AddOrderAsc(Expression<Func<T, object>> orderbyAsc)
        {

            OrderByAsc = orderbyAsc;
        }
        protected void AddOrderDesc(Expression<Func<T, object>> orderbyDesc)
        {
            OrderByDesc = orderbyDesc;
        }


    }
}
