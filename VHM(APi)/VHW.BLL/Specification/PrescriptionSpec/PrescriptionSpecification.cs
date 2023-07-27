using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.PrescriptionSpec
{
    public class PrescriptionSpecification:BaseSpecification<Prescription>
    {
        public PrescriptionSpecification(string id, BaseFilterationParams PrescriptionParams)
          : base(p => (string.IsNullOrEmpty(PrescriptionParams.Search) || p.Doctor.UserName.ToLower().Contains(PrescriptionParams.Search))
         && string.IsNullOrEmpty(id) || p.PatientId == id)
        {
            AddInclude(f => f.Doctor);
            AddInclude(f => f.Patient);
            AddOrderByDesc(p => p.CreationDate);
            ApplyPagination(PrescriptionParams.PageSize * (PrescriptionParams.PageIndex - 1), PrescriptionParams.PageSize);

        }
        public PrescriptionSpecification(int PrescriptionId) : base(s => s.Id == PrescriptionId)
        {
            AddInclude(r => r.Doctor);
            AddInclude(r => r.Patient);
        }
    }
}
