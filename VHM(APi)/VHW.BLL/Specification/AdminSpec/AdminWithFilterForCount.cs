using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities;

namespace VHW.BLL.Specification.AdminSpec
{
    public class AdminWithFilterForCount : BaseSpecification<Admin>
    {
        public AdminWithFilterForCount(BaseFilterationParams adminParams)
         : base(p => (string.IsNullOrEmpty(adminParams.Search) || (p.UserName.ToLower().Contains(adminParams.Search)))
                )
        {
        }
    }
}
