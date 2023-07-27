using System;

namespace VHM_APi_.Dtos.ConsultationTask
{
    public class ConsultationDto
    {
        public int Id { get; set; }   
        public string TaskType { get; set; }
        public DateTime date { get; set; }
        public int To { get; set; }

    }
}
