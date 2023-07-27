using VHM.DAL.Entities.PatientEntities;

namespace VHM_APi_.Helper
{
    public class ClassifiedData
    {
        public PatientStatus classification {  get; set; } 
        public int DeviceId { get; set; }

    }
}
