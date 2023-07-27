using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ReadingEntities
{
    [Table("DiabetsFeatures")]
    public class DiabetsFeatures: DiseaseFeatures
    {
        public float Insulin { get; set; }
        public float Glucose { get; set; }
        public float BMI { get; set; }
        //public int Age { get; set; }
        public float SkinThickness { get; set; }

    }
}
