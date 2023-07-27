using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ReadingEntities;

namespace VHW.BLL.Specification.PatientSpec
{
    public class PatientParams: BaseFilterationParams
    {
        public Disease? SearchByDisease { get; set; }
       
    }

}
