using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ReadingEntities
{
    [Table("BrainStroke")]
    public class BrainStroke: DiseaseFeatures
    {
        public float AvgGlucose { get; set; }
        public float BMI { get; set; }
        public float Hypertension { get; set; }
        public int HeartDisease { get; set; }

    }
}
