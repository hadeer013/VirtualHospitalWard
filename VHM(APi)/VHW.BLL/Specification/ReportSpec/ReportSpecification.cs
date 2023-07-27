using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.ReportSpec
{
    public class ReportSpecification : BaseSpecification<Report>
    {
        public ReportSpecification(string id, BaseFilterationParams ReportParams)
          : base(p => (string.IsNullOrEmpty(ReportParams.Search) || p.Doctor.UserName.ToLower().Contains(ReportParams.Search))
         &&string.IsNullOrEmpty(id) || p.PatientId== id)
        {
            AddInclude(f => f.Doctor);
            AddInclude(f => f.Patient);
            AddOrderByDesc(p => p.CreationDate);
            ApplyPagination(ReportParams.PageSize * (ReportParams.PageIndex - 1), ReportParams.PageSize);

        }
        public ReportSpecification(int ReportId) : base(s => s.Id == ReportId)
        {
            AddInclude(r => r.Doctor);
            AddInclude(r => r.Patient);
        }
    }
}
