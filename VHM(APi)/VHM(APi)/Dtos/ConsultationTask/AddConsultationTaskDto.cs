using System;

namespace VHM_APi_.Dtos.ConsultationTask
{
    public class AddConsultationTaskDto
    {
        //public int taskId { get; set; }
        public string TaskType { get; set; }
        public string userRecieverName { get; set; }
        //public string userRecieverName { get; set; }
        //public string userSenderName { get; set; }
        //public string userSenderId { get; set; }
        public int To { get; set; }
        public DateTime TaskDate { get; set; }
       // public MeetingStatus Status { get; set; }

    }
}
