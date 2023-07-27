using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ReadingEntities
{
    [Table("Cardiovascular")]
    public class Cardiovascular : DiseaseFeatures
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public float Ap_hi { get; set; }
        public float Ap_lo { get; set; }
        public float Cholesterol { get; set; }

    }
}
