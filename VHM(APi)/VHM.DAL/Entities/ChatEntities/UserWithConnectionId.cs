using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ChatEntities
{
    public class UserWithConnectionId
    {
        public UserWithConnectionId()
        {
        }

        public UserWithConnectionId(string userId, string connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }

        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
