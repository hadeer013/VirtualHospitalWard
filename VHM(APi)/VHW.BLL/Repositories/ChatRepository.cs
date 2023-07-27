using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Repositories
{
    public class ChatRepository : GenericRepository<Message>, IChatRepository
    {
        private readonly HospitalContext hospitalContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPatientRepository patientRepository;

        public ChatRepository(HospitalContext hospitalContext,UserManager<ApplicationUser> userManager,IPatientRepository patientRepository) : base(hospitalContext)
        {
            this.hospitalContext = hospitalContext;
            this.userManager = userManager;
            this.patientRepository = patientRepository;
        }

        private async Task<IReadOnlyList<ApplicationUser>> GetChatUsersForPatient()
        {
            var result =await hospitalContext.UserRoles.Join(
                hospitalContext.Roles,
                u => u.RoleId,
                r => r.Id,
                (u, r) => new
                {
                    UserId = u.UserId,
                    RoleName = r.Name
                }).Where(r => r.RoleName != "Patient"/* && r.RoleName != "Admin"*/).Join(hospitalContext.Users,
                u => u.UserId,
                s => s.Id,
                (u, s) =>s as ApplicationUser ).ToListAsync();
            return result;
        }//private
        private async Task<IReadOnlyList<ApplicationUser>> GetChatUsers(string UserId) //private
        {
            return await hospitalContext.Users.Where(u => u.Id != UserId).ToListAsync();
        }


        public async Task<IReadOnlyList<ApplicationUser>> GetNewUsersForMessaging(IReadOnlyList<string> userIds)
        {
            IEnumerable<ApplicationUser> result = new List<ApplicationUser>();
            foreach(var i in userIds)
            {
                var user = await userManager.FindByIdAsync(i);
                result = result.Append(user);
            }
            return result.ToList();
        }
        public async Task<IReadOnlyList<string>> GetNewUsersIdForPatient(ApplicationUser User)
        {
            var allUsers = await GetChatUsersForPatient();
            var AllusersIds = allUsers.Select(s => s.Id);
            var chattedUsers = await GetLatestUsersWithUnreadCount(User);
            var newUsersIds = AllusersIds.Except(chattedUsers.Select(u => u.UserId)).ToList();
            return newUsersIds;
        }

        public async Task<IReadOnlyList<string>> GetNewUsersId(ApplicationUser User)
        {
            var allUsers = await GetChatUsers(User.Id);
            var AllusersIds = allUsers.Select(s => s.Id);
            var chattedUsers = await GetLatestUsersWithUnreadCount(User);
            var newUsersIds = AllusersIds.Except(chattedUsers.Select(u => u.UserId)).ToList();
            return newUsersIds;
        }
       

        public async Task<IReadOnlyList<Message>> GetMessages(string SenderId, string ReceiverId)
        {
            hospitalContext.Messages.Where(m => !m.IsRead).ToList().ForEach(m => m.IsRead = true);

            return await hospitalContext.Messages.Include(m=>m.SenderUser).Include(m=>m.ReceiverUser)
                .Where(m => (m.SenderUserId == SenderId && m.ReceiverUserId == ReceiverId) 
                || (m.ReceiverUserId == SenderId && m.SenderUserId == ReceiverId)).OrderBy(m => m.date).ToListAsync();
        }

        public async Task<int> UpdateMessagesToRead(string SenderId, string ReceiverId)
        {
            
            var msgs = await GetMessages(SenderId, ReceiverId);
            var unReadMsgs = msgs.Where(m => !m.IsRead).ToList();
            foreach(var i in unReadMsgs)
            {
                i.IsRead = true;
                hospitalContext.Entry(i).State = EntityState.Detached;
                hospitalContext.Update(i);
            }
            return await hospitalContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<UserWithUnreadCount>> GetLatestUsersWithUnreadCount(ApplicationUser User)
        {
            var msgs = await hospitalContext.Messages
               .Where(m => m.SenderUserId == User.Id || m.ReceiverUserId == User.Id).ToListAsync();
            var latestMessages = msgs.AsEnumerable().OrderByDescending(m => m.date)
               .GroupBy(m => m.SenderUserId == User.Id ? m.ReceiverUserId : m.SenderUserId)
               .Take(10);

            var usersWithUnreadCount = latestMessages.Select(g => new UserWithUnreadCount
            {
                UserId = g.Key,
                UnreadCount = g.Count(m => m.ReceiverUserId == User.Id && !m.IsRead)
            }).ToList();
            return usersWithUnreadCount;
        }
        
    }
}
