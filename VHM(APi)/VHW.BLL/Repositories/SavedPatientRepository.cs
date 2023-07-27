using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories
{
    public class SavedRepository: ISavedRepository
    {
        private readonly HospitalContext context;

        public SavedRepository(HospitalContext context) 
        {
            this.context = context;
        }
        public async Task<int>Add(UserWithSavedPatient savedPatient)
        {
            context.Add(savedPatient);
            return await context.SaveChangesAsync();
        }
        public async Task<int>  Delete(UserWithSavedPatient savedPatient)
        {
            context.savedPatients.Remove(savedPatient);
            return await context.SaveChangesAsync();
        }
        public async Task<bool>CheckIfPatientAlreadyAddedToSaved(string UserId,string PatientId)
        {
            var result = await context.savedPatients.FindAsync(PatientId, UserId);
            if (result != null)
                return true;
            return false;
        }

        
        
    }
}
