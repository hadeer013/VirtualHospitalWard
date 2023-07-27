using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VHM.DAL.Entities;
using System.Linq;
using VHM.DAL.Data;
using VHM_APi_.Dtos;
using VHW.BLL.Interfaces;
using VHM.DAL.Entities.ChatEntities;
using JWT.Builder;

namespace VHM_APi_.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static Dictionary<string, string> connectionMap = new Dictionary<string, string>();
        private readonly static List<ApplicationUser> OnlineUsers = new List<ApplicationUser>();
        private static Dictionary<string, int> unreadMessages = new Dictionary<string, int>(); //string=>userName ,int=>numberOfUnreadMsgs
        private readonly HospitalContext hospitalcontext;
        private readonly IChatRepository chatRepo;
        private readonly IGenericRepository<Notification> notificationRepo;
        private readonly IHubContext<NotificationHub> notificationContext;

        public ChatHub(HospitalContext hospitalcontext, IChatRepository chatRepo,IGenericRepository<Notification> notificationRepo,IHubContext<NotificationHub>notificationContext)
        {
            this.hospitalcontext = hospitalcontext;
            this.chatRepo = chatRepo;
            this.notificationRepo = notificationRepo;
            this.notificationContext = notificationContext;
        }
        public async Task SendMessagePrivate(string ReceiverName, string Message)
        {
            var Receiver = hospitalcontext.Users.Where(s => s.UserName == ReceiverName).First();
            var Sender = hospitalcontext.Users.Where(s => s.UserName == IdentityName()).First();
            var msg = new Message()
            {
                Contetnt = Message,
                ReceiverUserId = Receiver.Id,
                SenderUserId = Sender.Id
            };
            await chatRepo.Add(msg);
            var MessageDto = new MessageDto()
            {
                Id = msg.Id,
                Contetnt = msg.Contetnt,
                date = msg.date,
                SenderName = Sender.UserName,
                RecieverName = Receiver.UserName,
                IsSent = true
            };

            if (unreadMessages.ContainsKey(ReceiverName))
            {
                unreadMessages[ReceiverName]++;
            }
            else
            {
                unreadMessages.Add(ReceiverName, 1);
            }

            if (connectionMap.TryGetValue(ReceiverName, out string ConnectionId))
            {
                await Clients.Clients(ConnectionId,Context.ConnectionId).SendAsync("NewMsg", msg.Contetnt, msg.date, Sender.UserName);
                //NewMsgSender
                //await Clients.Caller.SendAsync("NewMsg", msg.Contetnt, msg.date, Sender.UserName);


                

                await Clients.Clients(ConnectionId,Context.ConnectionId).SendAsync("ICon_PoppedUp", Sender.Id,Receiver.Id);
            }
            else
            {
                var notf = new Notification() { Content = $"{Sender.UserName} has sent you a new message", date = DateTime.Now, SenderId = Sender.Id, RecieverId = Receiver.Id};
                await notificationRepo.Add(notf);

                var userWithConnectionId = hospitalcontext.UserWithConnectionIds.Where(u => u.UserId == Receiver.Id).FirstOrDefault();
                if (userWithConnectionId != null)
                    await notificationContext.Clients.Client(userWithConnectionId.ConnectionId).SendAsync("ReceiveNotification", $"{Sender.UserName} has sent you a message", MessageDto.date, Sender.Id);
                
                await Clients.Caller.SendAsync("NewMsg", msg.Contetnt, msg.date, Sender.UserName);
                await Clients.Caller.SendAsync("ICon_PoppedUp", Sender.Id, Receiver.Id);
            }
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                var userName = IdentityName();
                var user = hospitalcontext.Users.Where(s => s.UserName == userName).FirstOrDefault();
                if (!OnlineUsers.Any(s => s.UserName == userName))
                {
                    OnlineUsers.Add(user);
                    connectionMap.Add(userName, Context.ConnectionId);
                }
                if (!unreadMessages.ContainsKey(IdentityName()))
                {
                    unreadMessages.Add(IdentityName(), 0);
                }

            }
            catch (Exception e)
            {
                Clients.Caller.SendAsync("ErrorMsg", "Error occur while connecting to hub : " + e.Message);
            }
            return base.OnConnectedAsync();
        } //baseUrl/chat
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = OnlineUsers.Where(s => s.UserName == IdentityName()).FirstOrDefault();
                OnlineUsers.Remove(user);
                connectionMap.Remove(IdentityName());

            }
            catch (Exception e)
            {
                Clients.Caller.SendAsync("ErrorMsg", "Error occur while disconnecting from hub : " + e.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }
        private string IdentityName()
            => Context.User.Identity.Name;
    }
}

