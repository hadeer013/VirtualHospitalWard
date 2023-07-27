using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHM.DAL.Entities.ReadingEntities
{
    [Table("DiseaseFeatures")]
    public class DiseaseFeatures:BaseEntity
    {
        public Patient Patient { get; set; }    
    }
}
