using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Interfaces;

namespace VHW.BLL.Repositories
{
    public class SupportMessagesRepository : GenericRepository<SupportMessages>, ISupportMessageRepository
    {
        public SupportMessagesRepository(HospitalContext hospitalContext) : base(hospitalContext)
        {
        }
    }
}
