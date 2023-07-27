using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Specification.SupportMessagesSpec;

namespace VHW.BLL.Specification
{
    public class SupportMessagesSpecification: BaseSpecification<SupportMessages>
    {
        public SupportMessagesSpecification(SupportMessParams SupportMessParams)
          : base(p => (string.IsNullOrEmpty(SupportMessParams.Search) || (p.Patient.UserName.ToLower().Contains(SupportMessParams.Search))
          ))
        {
            AddInclude(f => f.Patient);
            AddOrderBy(p => p.CreationDate);
            ApplyPagination((SupportMessParams.PageSize * (SupportMessParams.PageIndex - 1)), SupportMessParams.PageSize);

        }
        public SupportMessagesSpecification(int id) : base((Message) => Message.Id == id)
        {
            AddOrderBy(p => p.CreationDate);
            AddInclude(f => f.Patient);
        }
    }
}
