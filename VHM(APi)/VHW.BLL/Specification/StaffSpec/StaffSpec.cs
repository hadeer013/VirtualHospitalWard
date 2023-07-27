using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification.StaffSpec
{
    public class StaffSpec : BaseSpecification<Staff>
    {
        public StaffSpec(PatientParams StaffParams)
          : base(p => (string.IsNullOrEmpty(StaffParams.Search) || (p.UserName.ToLower().Contains(StaffParams.Search)))
                 )
        {
            AddOrderBy(p => p.UserName);
            ApplyPagination((StaffParams.PageSize * (StaffParams.PageIndex - 1)), StaffParams.PageSize);

        }
        public StaffSpec(string id) : base((staff) => staff.Id == id)
        {

        }
    }
}
