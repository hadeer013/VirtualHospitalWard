using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Interfaces
{
    public interface IReportRepository:IGenericRepository<Report>
    { 
        Task<IReadOnlyList<Report>> GetReportsByPatientId(string Patientid);
    }
}
