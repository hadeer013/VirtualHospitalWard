using System.Collections.Generic;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi_.Dtos;

namespace VHM_APi_.Helper
{
    public class Dashboard
    {
       

        public Dashboard(int pageIndex, int pageSize, int count,
            int TotalPatients, int StablePatients, int UnStablePatients, int WarningPatients
            , IReadOnlyList<PatientDto> Patients, IReadOnlyList<PatientDto> UnStablePatientList
            )
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.count = count;
            this.TotalPatients = TotalPatients;
            this.StablePatients = StablePatients;
            this.UnStablePatients = UnStablePatients;
            this.WarningPatients = WarningPatients;
            this.Patients = Patients;
            this.UnStablePatientList = UnStablePatientList;
        }
        public int pageIndex;
        public int pageSize;
        public int count;
        public int TotalPatients { get; set; }
        public int StablePatients { get; set; }
        public int UnStablePatients { get; set; }
        public int WarningPatients { get; set; }
        public IReadOnlyList<PatientDto> Patients { get; set; }
        public IReadOnlyList<PatientDto> UnStablePatientList { get; set; }

    }
}
