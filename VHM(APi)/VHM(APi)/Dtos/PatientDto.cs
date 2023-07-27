using VHM.DAL.Entities;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHM_APi_.EntityInputs;

namespace VHM_APi_.Dtos
{
    public class PatientDto:ApplicationUserDto
    {
        public Disease Disease { get; set; }
        public PatientStatus PatientStatus { get; set; }
    }
}
