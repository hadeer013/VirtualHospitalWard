using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.Ambulance
{
    public enum RequestStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Approved")]
        Approved
    }
}
