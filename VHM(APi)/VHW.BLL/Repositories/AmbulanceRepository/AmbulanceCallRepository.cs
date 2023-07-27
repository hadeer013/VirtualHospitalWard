using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.Ambulance;
using VHW.BLL.Interfaces;

namespace VHW.BLL.Repositories.AmbulanceRepository
{
    public class AmbulanceCallRepository : GenericRepository<AmbulanceCall>,IAmbulanceRepository
    {
        private readonly HospitalContext hospitalContext;

        public AmbulanceCallRepository(HospitalContext hospitalContext) : base(hospitalContext)
        {
            this.hospitalContext = hospitalContext;
        }

        public async Task<IReadOnlyList<AmbulanceCall>> GetAmbulanceCallForSpecificPatient(string PatientId)
        {
            var result = await hospitalContext.AmbulanceCalls.Where(a => a.PatientId == PatientId).ToListAsync();
            return result;
        }
    }
}
