using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;

using VHW.BLL.Interfaces;
using VHW.BLL.Specification;
using VHW.BLL.Statistics;

namespace VHW.BLL.Repositories
{
    public class PatientRepository : UserRepository<Patient>,IPatientRepository
    {
        private readonly HospitalContext context;
        private readonly IDoctorRepository doctorRepository;

        public PatientRepository(HospitalContext context,IDoctorRepository doctorRepository,UserManager<ApplicationUser> userManager):base(context,userManager)
        {
            this.context = context;
            this.doctorRepository = doctorRepository;
        }


        #region Saved Patients
        public async Task<IReadOnlyList<Patient>> GetSavedPatientWithSpesc(string UserId, ISpecification<Patient> spec)
        {
            return await ApplySpecificationForSavedPatients(UserId, spec).ToListAsync();
        }
        public async Task<int> GetCountForSavedPatientAsync(string UserId, ISpecification<Patient> spec)
        {
            return await ApplySpecificationForSavedPatients(UserId,spec).CountAsync();
        }

        private  IQueryable<Patient> ApplySpecificationForSavedPatients(string UserId, ISpecification<Patient> spec)
        {
            var saveQueryd = GetSavedPatientIDByUserId(UserId);
            return SpecificationEvaluator<Patient>.BuildQuery(saveQueryd, spec);
        }
        private IQueryable<Patient> GetSavedPatientIDByUserId(string UserId)
        {
            var result = from s in context.savedPatients
                         join p in context.Patients
                         on s.PatientId equals p.Id
                         where s.UserId == UserId
                         select p;
            return result;
        }

        public async Task<IList<Doctor>> GetResponsibleDoctors(string PatientId)
        {
            var ReportdoctorIDs = context.Reports.Where(r => r.PatientId == PatientId).Select(r => r.DoctorId).Distinct().ToList();
            var PrescriptiondoctorIDs = context.Prescriptions.Where(p => p.PatientId == PatientId).Select(p => p.DoctorId).Distinct().ToList();
            var doctorIds = ReportdoctorIDs.Union(PrescriptiondoctorIDs).ToList();
            IList<Doctor> doctors=new List<Doctor>();
            foreach(var i in doctorIds)
            {

                var doc = await context.Doctors.FindAsync(i);
                doctors.Add(doc);
            }
            return doctors;
        }


        #endregion



        #region OLD
        //public async Task<PatientsStatistics> GetPatientsStatistics()
        //{
        //    var stable = await GetPatientsByStatus(PatientStatus.Stable);
        //    var unstable= await GetPatientsByStatus(PatientStatus.UnStable);
        //    var warning= await GetPatientsByStatus(PatientStatus.Warning);
        //    return new PatientsStatistics()
        //    {
        //        TotalPatientCount=context.Patients.ToList().Count(),
        //        StablePatientCount = stable.Count(),
        //        UnStablePatientCount = unstable.Count(),
        //        WarningPatientCount = warning.Count(),
        //        UnStablePatients= (ICollection<Patient>)unstable
        //    };
        //}

        //public async Task<IEnumerable<Patient>> GetPatientsByStatus(PatientStatus status)
        //{
        //    switch (status)
        //    {
        //        case PatientStatus.Stable:
        //            return await context.Patients.Include(p=>p.Address).Where(p => p.Status == PatientStatus.Stable).ToListAsync();
        //        case PatientStatus.UnStable:
        //            return await context.Patients.Include(p => p.Address).Where(p => p.Status == PatientStatus.UnStable).ToListAsync();
        //        case PatientStatus.Warning:
        //            return await context.Patients.Include(p => p.Address).Where(p => p.Status == PatientStatus.Warning).ToListAsync();
        //        default:
        //            return await context.Patients.Include(p => p.Address).ToListAsync();
        //    }
        //}

        #endregion


    }
}
