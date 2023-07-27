using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;

namespace VHW.BLL.Interfaces
{
    public interface ISavedRepository
    {
        Task<int> Add(UserWithSavedPatient savedPatient);
        Task<bool> CheckIfPatientAlreadyAddedToSaved(string UserId, string PatientId);
        Task<int> Delete(UserWithSavedPatient savedPatient);
    }
}
