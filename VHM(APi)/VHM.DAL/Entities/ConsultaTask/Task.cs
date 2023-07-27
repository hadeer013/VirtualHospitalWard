using System;
using System.ComponentModel.DataAnnotations.Schema;
using VHM_APi_.DAL.ConsultaTask;

namespace VHM.DAL.Entities.ConsultaTask
{
    public class ConsultaionTask : BaseEntity
    {
        public string TaskType { get; set; }
        public string UserRecieverId { get; set; }
        [ForeignKey(nameof(UserRecieverId))]
        public ApplicationUser UserReciever { get; set; }

        public string UserInitializerId { get; set; }
        [ForeignKey(nameof(UserInitializerId))]
        public ApplicationUser UserInitializer { get; set; }

        public DateTime TaskDate { get; set; }
        public int To { get; set; }

        public MeetingStatus Status { get; set; } = MeetingStatus.Pending;
        public string JobId { get; set; }
    }
}
