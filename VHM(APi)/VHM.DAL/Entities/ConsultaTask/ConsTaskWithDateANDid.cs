using System;
using System.Collections.Generic;
using VHM.DAL.Entities.ConsultaTask;

namespace VHM_APi_.DAL.ConsultaTask
{
    public class ConsTaskWithDateANDid
    {
        public ICollection<ConsultaionTask> Tasks { get; set; }=new HashSet<ConsultaionTask>();
        public DateTime TaskDate { get; set; }
       
    }
}
