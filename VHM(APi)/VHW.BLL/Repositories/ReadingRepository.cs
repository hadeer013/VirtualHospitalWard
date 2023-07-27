using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHW.BLL.Interfaces;

namespace VHW.BLL.Repositories
{
    public class ReadingRepository : GenericRepository<Readings>, IReadingRepository
    {
        private readonly HospitalContext hospitalContext;

        public ReadingRepository(HospitalContext hospitalContext) : base(hospitalContext)
        {
            this.hospitalContext = hospitalContext;
        }

        public Task<Readings> GetRecentPatientReading(string PatientId)
        {
            var reading = hospitalContext.Readings.Where(r => r.PatientId == PatientId)
                                                  .OrderByDescending(r => r.ReadingDate)
                                                  .FirstOrDefaultAsync();
            return reading;
        }
        public async Task<IReadOnlyList<Readings>> GetAllPatientReadings(string PatientId)
        {
            var reading = await hospitalContext.Readings.Where(r => r.PatientId == PatientId )
                                                .OrderByDescending(r => r.ReadingDate)
                                                .ToListAsync();
            return reading;
        }
    }
}
