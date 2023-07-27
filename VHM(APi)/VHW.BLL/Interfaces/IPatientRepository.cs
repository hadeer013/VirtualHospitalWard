using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Specification;
using VHW.BLL.Statistics;

namespace VHW.BLL.Interfaces
{
    public interface IPatientRepository : IUserRepository<Patient>
    {
        Task<IReadOnlyList<Patient>> GetSavedPatientWithSpesc(string UserId, ISpecification<Patient> spec);
        Task<int> GetCountForSavedPatientAsync(string UserId, ISpecification<Patient> spec);
        Task<IList<Doctor>> GetResponsibleDoctors(string PatientId);
    }
}
