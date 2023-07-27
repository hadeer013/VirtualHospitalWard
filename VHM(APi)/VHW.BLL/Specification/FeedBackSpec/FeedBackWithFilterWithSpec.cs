using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.FeedBackSpec
{
    public class FeedBackWithFilterWithSpec : BaseSpecification<FeedBack>
    {
        public FeedBackWithFilterWithSpec(FeedBackParams FeedbackParams)
          : base(p => (string.IsNullOrEmpty(FeedbackParams.Search) || (p.Patient.UserName.ToLower().Contains(FeedbackParams.Search))
          ) && (string.IsNullOrEmpty(FeedbackParams.StarNumber) || (p.RateValue == FeedbackParams.StarNumber)))
        { }
    }
}
