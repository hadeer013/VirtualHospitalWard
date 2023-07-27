using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ReadingEntities
{
    [Table("KidneyFeatures")]
    public class KidneyFeatures: DiseaseFeatures
    {
        public float whiteBloodCellCount { get; set; }
        public float BloodUrea { get; set; }
        public float BloodglucoseRandom { get; set; }
        public float SerumCreatinine { get; set; }
        public float PackedCellVolume { get; set; }
        public float Albumin { get; set; }
        public float Haemoglobin { get; set; }
        //public int Age { get; set; }
        public float Sugar { get; set; }
        public float Hypertension { get; set; }
    }
}
