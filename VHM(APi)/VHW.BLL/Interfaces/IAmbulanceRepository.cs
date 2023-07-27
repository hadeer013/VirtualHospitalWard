using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.Ambulance;

namespace VHW.BLL.Interfaces
{
    public interface IAmbulanceRepository:IGenericRepository<AmbulanceCall>
    {
       Task<IReadOnlyList<AmbulanceCall>> GetAmbulanceCallForSpecificPatient(string PatientId);
    }
}
