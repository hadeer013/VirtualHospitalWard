using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Interfaces;

namespace VHW.BLL.Repositories
{
    public class ReportRepository:GenericRepository<Report>, IReportRepository
    {
        private readonly HospitalContext context;

        public ReportRepository(HospitalContext context):base(context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<Report>> GetReportsByPatientId(string Patientid)
        {
            var result = await (from E in context.Patients.Include(p => p.Reports).ThenInclude(r => r.Doctor)
                                where E.Id == Patientid
                                select E.Reports).FirstOrDefaultAsync();

            return result.ToList();
        } //done

    }
}
