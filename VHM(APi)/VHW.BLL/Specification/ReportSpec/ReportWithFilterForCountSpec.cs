using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.ReportSpec
{
    public class ReportWithFilterForCountSpec:BaseSpecification<Report>
    {
        public ReportWithFilterForCountSpec(string id,BaseFilterationParams ReportParams)
          : base(p => (string.IsNullOrEmpty(ReportParams.Search) || p.Doctor.UserName.ToLower().Contains(ReportParams.Search))
         && string.IsNullOrEmpty(id) || p.PatientId == id)
        { }
    }
}
