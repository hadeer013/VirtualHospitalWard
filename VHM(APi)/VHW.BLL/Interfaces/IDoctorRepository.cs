using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Interfaces
{
    public interface IDoctorRepository:IUserRepository<Doctor>
    {
        Task<IReadOnlyList<Doctor>> GetOnlineDoctor();
    }
}
