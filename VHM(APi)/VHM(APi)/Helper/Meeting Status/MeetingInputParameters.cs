using System;

namespace VHM_APi_.Helper.Meeting_Status
{
    public class MeetingInputParameters
    {
        public int TaskId { get; set; }
        public MeetingStatus Status { get; set; }

        public int To { get; set; }
        public DateTime TaskDate { get; set;}
    }
}
