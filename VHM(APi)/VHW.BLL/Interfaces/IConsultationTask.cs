using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ConsultaTask;
using VHM_APi_.DAL.ConsultaTask;

namespace VHW.BLL.Interfaces
{
    public interface IConsultationTask:IGenericRepository<ConsultaionTask>
    {
        Task<IReadOnlyList<ConsTaskWithDateANDid>> GetConsultaionTasksForWeek(string UserId);
        
    }
}
