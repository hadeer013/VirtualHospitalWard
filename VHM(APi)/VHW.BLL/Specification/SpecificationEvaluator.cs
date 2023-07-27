using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHW.BLL.Specification;

namespace Talabat.BLL.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> BuildQuery(IQueryable<T> InputQuery,ISpecification<T> Spec)
        {
            var query = InputQuery;


            if(Spec.OrderBy !=null)
                query = query.OrderBy(Spec.OrderBy);
            if (Spec.OrderByDesc != null)
                query = query.OrderByDescending(Spec.OrderByDesc);

            if(Spec.IsPaginated)
             query = query.Skip(Spec.Skip).Take(Spec.Take);

            query = Spec.Includes.Aggregate(query, (start, include) => start.Include(include));


            if (Spec.Criteria != null)
                query = query.Where(Spec.Criteria);

            return query;

        }
    }
}
