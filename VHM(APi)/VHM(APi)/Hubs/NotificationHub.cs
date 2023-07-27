
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System;
using System.Formats.Asn1;
using System.Threading.Tasks;
using VHM.DAL.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using VHM.DAL.Entities;
using System.Linq;
using VHM.DAL.Entities.ChatEntities;

namespace VHM_APi_.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly HospitalContext hospitalContext;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationHub(HospitalContext hospitalContext,UserManager<ApplicationUser> userManager)
        {
            this.hospitalContext = hospitalContext;
            this.userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await userManager.FindByNameAsync(this.Context.User.Identity.Name);
            hospitalContext.UserWithConnectionIds.Add(new UserWithConnectionId(userId: user.Id, connectionId: this.Context.ConnectionId));
            await Clients.All.SendAsync("ConnectToHub", user.UserName);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await userManager.FindByNameAsync(this.Context.User.Identity.Name);
            var userWithConnection = hospitalContext.UserWithConnectionIds.Where(u => u.UserId == user.Id).FirstOrDefault();
            if(userWithConnection != null) { hospitalContext.UserWithConnectionIds.Remove(userWithConnection);await hospitalContext.SaveChangesAsync(); }
            await Clients.All.SendAsync("LeaveHub", user.UserName);
            await base.OnDisconnectedAsync(exception);
        }

       

        
    }

}
