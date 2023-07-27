using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ReadingEntities;

namespace VHW.BLL.Interfaces
{
    public interface IReadingRepository:IGenericRepository<Readings>
    {
        Task<Readings> GetRecentPatientReading(string PatientId);
        Task<IReadOnlyList<Readings>> GetAllPatientReadings(string PatientId);
    }
}
