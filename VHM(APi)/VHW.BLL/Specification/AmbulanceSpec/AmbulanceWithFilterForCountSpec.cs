using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.Ambulance;

namespace VHW.BLL.Specification.AmbulanceSpec
{
    public class AmbulanceWithFilterForCountSpec : BaseSpecification<AmbulanceCall>
    {
        public AmbulanceWithFilterForCountSpec(BaseFilterationParams callParams,string PatientId=null)
         : base(p => (string.IsNullOrEmpty(callParams.Search) || (p.Patient.UserName.ToLower().Contains(callParams.Search)))&&
          (string.IsNullOrEmpty(PatientId) || p.PatientId == PatientId))
        {
        }
    }
}
