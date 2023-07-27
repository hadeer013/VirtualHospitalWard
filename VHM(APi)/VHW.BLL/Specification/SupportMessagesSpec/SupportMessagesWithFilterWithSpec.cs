using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Specification.SupportMessagesSpec
{
    public class SupportMessagesWithFilterWithSpec : BaseSpecification<SupportMessages>
    {
        public SupportMessagesWithFilterWithSpec(SupportMessParams SupportMessParams)
          : base(p => (string.IsNullOrEmpty(SupportMessParams.Search) || (p.Patient.UserName.ToLower().Contains(SupportMessParams.Search))
          ))
        {
        }
    }
}
