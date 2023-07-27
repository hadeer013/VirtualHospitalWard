using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;

namespace VHW.BLL.Interfaces
{
    public interface IChatRepository:IGenericRepository<Message>
    {
       // Task<IReadOnlyList<ApplicationUser>> GetChatUsersForPatient();
        //Task<IReadOnlyList<ApplicationUser>> GetChatUsers(string UserId);
        Task<IReadOnlyList<Message>> GetMessages(string SenderId,string ReceiverId);
        Task<IReadOnlyList<UserWithUnreadCount>> GetLatestUsersWithUnreadCount(ApplicationUser User);
        Task<IReadOnlyList<string>> GetNewUsersIdForPatient(ApplicationUser User);
        Task<IReadOnlyList<string>> GetNewUsersId(ApplicationUser User);
        Task<IReadOnlyList<ApplicationUser>> GetNewUsersForMessaging(IReadOnlyList<string> userIds);
        Task<int> UpdateMessagesToRead(/*IReadOnlyList<Message> messages*/string SenderId, string ReceiverId);
    }
}
