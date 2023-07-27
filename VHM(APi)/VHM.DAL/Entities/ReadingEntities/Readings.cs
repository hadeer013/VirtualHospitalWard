using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHM.DAL.Entities.ReadingEntities
{
    public class Readings : BaseEntity
    {
        public Readings()
        {
        }

        public Readings(float BPM, float AvgBPM, float Temperature, int DeviceId, string PatientId)
        {
            this.BPM = BPM;
            this.AvgBPM = AvgBPM;
            this.Temperature = Temperature;
            this.DeviceId = DeviceId;
            this.PatientId = PatientId;
        }

        public float BPM { get; set; }
        public float AvgBPM { get; set; }
        public float Temperature { get; set; }
        public DateTime ReadingDate { get; set; } = DateTime.Now;
        public PatientStatus PatientStatus { get; set; } = PatientStatus.Stable;
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public string PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
    }
}
