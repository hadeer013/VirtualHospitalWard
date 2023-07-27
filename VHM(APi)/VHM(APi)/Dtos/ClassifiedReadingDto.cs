using VHM.DAL.Entities.PatientEntities;

namespace VHM_APi_.Dtos
{
    public class ClassifiedReadingDto:ReadingDto
    {
        public PatientStatus patientStatus { get; set; }
    }
}
