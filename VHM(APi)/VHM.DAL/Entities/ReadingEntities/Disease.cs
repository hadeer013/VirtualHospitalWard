using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ReadingEntities
{
    public enum Disease
    {
        [EnumMember(Value = "Cardiovascular")]
        Cardiovascular=0,
        [EnumMember(Value = "Diabets")]
        Diabets,
        [EnumMember(Value = "BrainStroke")]
        BrainStroke,
        [EnumMember(Value = "Kidney")]
        Kidney
    }
}
