using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.ConsultaTask;

namespace VHW.BLL.Specification.ConsultationTaskSpec
{
    public class ConsultationTaskSpecification:BaseSpecification<ConsultaionTask>
    {
        public ConsultationTaskSpecification(BaseFilterationParams taskParams)
         : base(p => (string.IsNullOrEmpty(taskParams.Search) || (p.UserInitializer.UserName.ToLower().Contains(taskParams.Search )
         || p.UserReciever.UserName.ToLower().Contains(taskParams.Search))))
        {
            AddInclude(f => f.UserInitializer);
            AddInclude(f => f.UserReciever);
            AddOrderByDesc(p => p.TaskDate);
            ApplyPagination(taskParams.PageSize * (taskParams.PageIndex - 1), taskParams.PageSize);

        }
        public ConsultationTaskSpecification(int taskId) : base(s => s.Id == taskId)
        {
            AddInclude(r => r.UserReciever);
            AddInclude(r => r.UserInitializer);
        }
    }
}
