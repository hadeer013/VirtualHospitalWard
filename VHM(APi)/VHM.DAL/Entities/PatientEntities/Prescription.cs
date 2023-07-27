using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;


namespace VHM.DAL.Entities.PatientEntities
{
    public class Prescription: BaseEntity
    {   
       
        public string Content { get; set; }

        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public DateTime CreationDate { get; set; }=DateTime.Now;
        
    }
}
