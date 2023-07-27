using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.DoctorEntities
{
    public class UserWithSavedPatient
    {
        [Column(Order =0)]
        public string UserId { get; set; }
        [Column(Order = 1)]
        public string PatientId { get; set; }
    }
}
