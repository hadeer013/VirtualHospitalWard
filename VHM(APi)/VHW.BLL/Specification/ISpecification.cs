using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VHW.BLL.Specification
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }//where
        public List<Expression<Func<T, object>>> Includes { get; set; }//include

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginated { get; set; }
    }
}
