using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification.AdminSpec
{
    public class AdminSpecification:BaseSpecification<Admin>
    {
        public AdminSpecification(BaseFilterationParams adminParams)
          : base(p => (string.IsNullOrEmpty(adminParams.Search) || (p.UserName.ToLower().Contains(adminParams.Search)))
                 )
        {
            AddOrderBy(p => p.UserName);
            ApplyPagination((adminParams.PageSize * (adminParams.PageIndex - 1)), adminParams.PageSize);

        }
        public AdminSpecification(string id) : base((admin) => admin.Id == id)
        {

        }
    }
}
