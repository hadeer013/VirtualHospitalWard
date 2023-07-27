using System;
using VHM_APi_.Helper;

namespace VHM_APi_.Dtos.ConsultationTask
{
    public class ConsultationDetailsDto
    {
        public int Id { get; set; }   //
        public string TaskType { get; set; }//
        public DateTime TaskDate { get; set; }
        public string RecieverName { get; set; }
        public int To { get; set; }//
        public MeetingStatus Status { get; set; }

    }
}
