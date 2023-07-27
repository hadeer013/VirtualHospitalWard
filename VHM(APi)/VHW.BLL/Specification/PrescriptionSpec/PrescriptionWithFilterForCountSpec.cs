using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.PrescriptionSpec
{
    public class PrescriptionWithFilterForCountSpec:BaseSpecification<Prescription>
    {
        public PrescriptionWithFilterForCountSpec(string id, BaseFilterationParams PrescriptionParams)
          : base(p => (string.IsNullOrEmpty(PrescriptionParams.Search) || p.Doctor.UserName.ToLower().Contains(PrescriptionParams.Search))
         && string.IsNullOrEmpty(id) || p.PatientId == id)
        { }
    }
}
