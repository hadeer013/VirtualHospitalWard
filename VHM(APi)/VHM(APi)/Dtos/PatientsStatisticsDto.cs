using System.Collections.Generic;

namespace VHM_APi_.Dtos
{
    public class PatientsStatisticsDto
    {
        public int TotalPatientCount { get; set; }
        public int StablePatientCount { get; set; }
        public int UnStablePatientCount { get; set; }
        public int WarningPatientCount { get; set; }
        public virtual ICollection<PatientDto> UnStablePatients { get; set; } = new HashSet<PatientDto>();
    }
}
