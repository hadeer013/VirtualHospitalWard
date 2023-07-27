using System;
using System.Collections.Generic;
using VHM_APi_.Dtos.ConsultationTask;

namespace VHM_APi_.Helper
{
    public class AllTasksObject
    {
        public DateTime TaskDate { get; set; }
        public int Count { get; set; }
        public ICollection<ConsultationDto> Consultations { get; set; }=new HashSet<ConsultationDto>();

    }
}
