using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> BuildQuery(IQueryable<TEntity> inputquery, ISpecifications<TEntity> spec)
        {
            var outputquery = inputquery;
            if (spec.Criteria is not null)
            {
                outputquery = outputquery.Where(spec.Criteria);
            }

            if (spec.OrderByAsc is not null)
            {
                outputquery = outputquery.OrderBy(spec.OrderByAsc);
            }
            else if (spec.OrderByDesc is not null)
            {
                outputquery = outputquery.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsEnabledPagination)
            {
                outputquery = outputquery.Skip(spec.Skip).Take(spec.Take);
            }

            // inputquery.Where(p => p.Id == 1).OrderByDescending(p => p.Id).Include(p => p.Id).Skip(10).Take(5).OrderBy(p => p.Id);
            if (spec.Includes.Count() > 0)
            {
                outputquery = spec.Includes.Aggregate(outputquery, (CurrentQuery, Include) => CurrentQuery.Include(Include));
            }
            return outputquery;
        }
    }
}
