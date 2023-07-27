using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.PatientEntities
{
    public class SupportMessages:BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}
