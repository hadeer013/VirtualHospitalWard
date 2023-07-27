using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHM.DAL.Entities.DevicesEntities
{
    public class Device:BaseEntity
    {
        public string DeviceName { get; set; } = "Device";
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

    }
}
