using System;
using VHM.DAL.Entities.Ambulance;

namespace VHM_APi_.Dtos.ConsultationTask
{
    public class appointmetDto
    {
        public int TaskId { get; set; }
        public DateTime date { get; set; }
        public string InitiatorName { get; set; }
        public string InitiatorId { get; set; }
        public string ReceiverName { get; set; }
        public string RecieverId { get; set; }
        public MeetingStatus Status { get; set; }

    }
   
}
