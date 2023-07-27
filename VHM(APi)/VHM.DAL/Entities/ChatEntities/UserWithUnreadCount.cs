using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ChatEntities
{
    public class UserWithUnreadCount
    {
        public string UserId { get; set; }
        public int UnreadCount { get; set; }
    }
}
