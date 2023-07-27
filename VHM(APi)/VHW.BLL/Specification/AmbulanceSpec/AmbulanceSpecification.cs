using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.Ambulance;
using VHW.BLL.Specification.DoctorSpec;

namespace VHW.BLL.Specification.AmbulanceSpec
{
    public class AmbulanceSpecification:BaseSpecification<AmbulanceCall>
    {
        public AmbulanceSpecification(BaseFilterationParams callParams,string PatientId)
         : base(p => (string.IsNullOrEmpty(callParams.Search) || (p.Patient.UserName.ToLower().Contains(callParams.Search)))
         && (string.IsNullOrEmpty(PatientId) ||p.PatientId== PatientId))
        {
            AddInclude(a => a.Patient);
            AddOrderByDesc(a => a.Date);
            ApplyPagination((callParams.PageSize * (callParams.PageIndex - 1)), callParams.PageSize);
        }

        public AmbulanceSpecification(int CallId) : base(a=>a.Id==CallId)
        {
            AddInclude(a => a.Patient);
        }
    }
}
