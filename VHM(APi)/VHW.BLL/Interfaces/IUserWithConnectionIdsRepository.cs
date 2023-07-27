using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ChatEntities;

namespace VHW.BLL.Interfaces
{
    public interface IUserWithConnectionIdsRepository
    {
       Task<IReadOnlyList<UserWithConnectionId>> GetAll();
    }
}
