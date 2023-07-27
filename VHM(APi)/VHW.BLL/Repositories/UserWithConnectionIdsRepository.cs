using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities.ChatEntities;
using VHW.BLL.Interfaces;

namespace VHW.BLL.Repositories
{
    public class UserWithConnectionIdsRepository : IUserWithConnectionIdsRepository
    {
        private readonly HospitalContext hospitalContext;

        public UserWithConnectionIdsRepository(HospitalContext hospitalContext)
        {
            this.hospitalContext = hospitalContext;
        }

        public async Task<IReadOnlyList<UserWithConnectionId>> GetAll()
        {
            return await hospitalContext.UserWithConnectionIds.ToListAsync();
        }
    }
}
