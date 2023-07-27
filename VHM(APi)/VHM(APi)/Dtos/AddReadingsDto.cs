using System;
using System.ComponentModel.DataAnnotations;
using VHM.DAL.Entities.PatientEntities;

namespace VHM_APi_.Dtos
{
    public class AddReadingsDto
    {
        [Required]
        public int DeviceId{ get; set; }
        public float BPM { get; set; }
        public float AvgBPM { get; set; }
        public float Temperature { get; set; }
    }
}
