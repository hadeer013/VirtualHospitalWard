using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ConsultaTask;
using VHM.DAL.Entities.PatientEntities;


namespace VHM.DAL.Entities.DoctorEntities
{
    [Table("Doctors")]
    public class Doctor: ApplicationUser
    {
        public string Department { get; set; }

        
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        //public virtual ICollection<ConsultaionTask> ConsultaionTasks { get; set; }=new List<ConsultaionTask>();
    }
}
