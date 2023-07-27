using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.ConsultaTask;

namespace VHW.BLL.Specification.ConsultationTaskSpec
{
    public class ConsultationTaskwithFilterForCountSpec : BaseSpecification<ConsultaionTask>
    {
        public ConsultationTaskwithFilterForCountSpec(BaseFilterationParams taskParams)
         : base(p => (string.IsNullOrEmpty(taskParams.Search) || (p.UserInitializer.UserName.ToLower().Contains(taskParams.Search)
         || p.UserReciever.UserName.ToLower().Contains(taskParams.Search))))
        {
        }
    }
}
