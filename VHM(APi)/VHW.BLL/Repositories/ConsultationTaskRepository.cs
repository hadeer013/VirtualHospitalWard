using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.ConsultaTask;
using VHM_APi_.DAL.ConsultaTask;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories
{
    public class ConsultationTaskRepository : GenericRepository<ConsultaionTask>, IConsultationTask
    {
        private readonly HospitalContext context;

        public ConsultationTaskRepository(HospitalContext context) : base(context)
        {
            this.context = context;
        }
        
        public async Task<IReadOnlyList<ConsTaskWithDateANDid>> GetConsultaionTasksForWeek(string UserId)
        {
            var ConsTaskWithDateAND = new List<ConsTaskWithDateANDid>();
            var today = DateTime.Now.Date;
            DateTime week = today;
            for (int i = 0; i < 7; i++)
            {
                week = today.AddDays(i);
                var Task = await context.consultaionTasks.
                    Where(t => t.TaskDate == week && (t.UserInitializerId == UserId || t.UserRecieverId == UserId)).ToListAsync();
                ConsTaskWithDateAND.Add(new ConsTaskWithDateANDid() { Tasks = Task, TaskDate = week });
            }

            return ConsTaskWithDateAND;
        }



    }
}
