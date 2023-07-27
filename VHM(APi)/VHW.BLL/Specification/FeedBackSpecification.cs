using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Specification.FeedBackSpec;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification
{
    public class FeedBackSpecification: BaseSpecification<FeedBack>
    {
        public FeedBackSpecification(FeedBackParams FeedbackParams)
          : base(p => (string.IsNullOrEmpty(FeedbackParams.Search) || (p.Patient.UserName.ToLower().Contains(FeedbackParams.Search))
          )&&(string.IsNullOrEmpty(FeedbackParams.StarNumber)||(p.RateValue==FeedbackParams.StarNumber)))
        {
            AddInclude(f => f.Patient);
            AddOrderByDesc(p => p.CreationDate);
            ApplyPagination((FeedbackParams.PageSize * (FeedbackParams.PageIndex - 1)), FeedbackParams.PageSize);

        }
        public FeedBackSpecification(int id) : base((feedback) => feedback.Id == id)
        {
            AddOrderBy(p => p.CreationDate);
            AddInclude(f => f.Patient);
        }
    }
}
