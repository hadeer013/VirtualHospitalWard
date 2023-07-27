using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.PatientEntities
{
    public enum PatientStatus
    {
        [EnumMember(Value = "Stable")]
        Stable=0,
        [EnumMember(Value = "Unstable")]
        Unstable=1,

    }
}
