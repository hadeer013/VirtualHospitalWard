using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DevicesEntities;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM.DAL.Entities.PatientEntities
{
    [Table("Patients")]
    public class Patient:ApplicationUser
    {
        public Disease Disease { get; set; }
        public PatientStatus PatientStatus { get; set; }
        
        public int? DiseaseFeaturesId { get;set; }

        public Device Device { get; set; }

        [ForeignKey(nameof(DiseaseFeaturesId))]
        public DiseaseFeatures DiseaseFeatures { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Readings> Readings { get; set; } = new List<Readings>(); 
        public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<SupportMessages> SupportMessages { get; set; } = new List<SupportMessages>();
        
    }
}
