using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.DoctorEntities;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification.DoctorSpec
{
    public class DoctorWithFilterForCountSpec : BaseSpecification<Doctor>
    {
        public DoctorWithFilterForCountSpec(DoctorParams DoctorParams)
          : base(p => (string.IsNullOrEmpty(DoctorParams.Search) || (p.UserName.ToLower().Contains(DoctorParams.Search)))
                 && string.IsNullOrEmpty(DoctorParams.Department) || p.Department.ToLower().Contains(DoctorParams.Department))
        {
        }
    }
}
