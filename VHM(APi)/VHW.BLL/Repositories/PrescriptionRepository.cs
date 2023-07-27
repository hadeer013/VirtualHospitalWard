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
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        private readonly HospitalContext context;

        public PrescriptionRepository(HospitalContext Context) : base(Context)
        {
            context = Context;
        }
        public async Task<IReadOnlyList<Prescription>> GetPrescriptionsByPatientId(string Patientid)
        {
            var result = await (from E in context.Patients.Include(p => p.Prescriptions).ThenInclude(r => r.Doctor)
                                where E.Id == Patientid
                                select E.Prescriptions).FirstOrDefaultAsync();

            return result.ToList();
        } //done
    }
}
