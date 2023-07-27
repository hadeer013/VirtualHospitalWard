using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.DevicesEntities;

namespace VHW.BLL.Specification.DeviceSpec
{
    public class DeviceSpecification:BaseSpecification<Device>
    {
        public DeviceSpecification(BaseFilterationParams devParams)
        {
            ApplyPagination((devParams.PageSize * (devParams.PageIndex - 1)), devParams.PageSize);
            AddInclude(D => D.Patient);
        }
        public DeviceSpecification(int id) : base((dev) => dev.Id == id)
        {
            AddInclude(D => D.Patient);
        }
    }
}
