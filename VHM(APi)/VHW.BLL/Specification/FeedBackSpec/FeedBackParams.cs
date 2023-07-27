using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification.FeedBackSpec
{
    public class FeedBackParams:BaseFilterationParams
    {
        public string StarNumber { get; set; }
    }
}
