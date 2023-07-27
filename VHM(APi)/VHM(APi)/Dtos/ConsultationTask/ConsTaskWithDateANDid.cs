using System;
using System.Collections.Generic;

namespace VHM_APi_.Dtos.ConsultationTask
{
    public class ConsTaskWithDateANDid
    {
        public List<int> Ids { get; set; } = new List<int>();
        public DateTime TaskDate { get; set; }
    }
}
