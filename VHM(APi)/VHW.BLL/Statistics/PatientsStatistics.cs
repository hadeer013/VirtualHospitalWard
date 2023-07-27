using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Statistics
{
    public class PatientsStatistics
    {
        public int TotalPatientCount { get; set; }
        public int StablePatientCount { get; set; }
        public int UnStablePatientCount { get; set; }
        public int WarningPatientCount { get; set; }
        public virtual ICollection<Patient> UnStablePatients { get; set; } = new HashSet<Patient>();
    }
}
