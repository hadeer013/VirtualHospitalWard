using VHM.DAL.Entities.ReadingEntities;

namespace VHM_APi_.Dtos.VideoCall
{
    public class DataSentToModelDto
    {
        public AddReadingsDto addReadingsDto { get; set; }
        public DiseaseFeatures DiseaseFeatures { get; set; }
        public Disease Disease { get; set; }
        public int Age { get;set; }
    }
}
