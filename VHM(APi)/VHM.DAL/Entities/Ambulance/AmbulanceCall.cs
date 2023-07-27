using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHM.DAL.Entities.Ambulance
{
    public class AmbulanceCall:BaseEntity
    {
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
        public PatientLocation Location { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}
