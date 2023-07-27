using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;
using VHM_APi_.Dtos.Chat;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification.NotificationSpec;

namespace VHM_APi_.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepository<Notification> notificationRepo;
        private readonly IMapper mapper;

        public NotificationController(UserManager<ApplicationUser> userManager,IGenericRepository<Notification> notificationRepo
            ,IMapper mapper)
        {
            this.userManager = userManager;
            this.notificationRepo = notificationRepo;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<NotificationDto>>> GetAllNotification()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var NotificationSpec = new NotificationSpecification(user.Id);
            var result = await notificationRepo.GetAllWithSpec(NotificationSpec);
            //Get the notification which is marked as unread
            var mapped=mapper.Map<IReadOnlyList<NotificationDto>>(result);
            return Ok(mapped);
        }

        //GetAll notification  or  Get the notification which is marked as unread ??????

        [Authorize]
        [HttpGet("GetNumberOfUnreadNotification")]
        public async Task<ActionResult<int>> GetNumberOfUnreadNotification()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var NotificationSpec = new NotificationSpecification(user.Id);
            var notifications = await notificationRepo.GetAllWithSpec(NotificationSpec);
            var result=notifications.Where(n => !n.IsRead).ToList();
            return Ok(result.Count());
        }
        [Authorize]
        [HttpGet("markNotificationAsRead/{id}")]
        public async Task<ActionResult> markNotificationAsRead(int id)
        {
            var notif = await notificationRepo.GetWithId(id);
            notif.IsRead = true;
            await notificationRepo.Update(notif);
            return Ok(mapper.Map<Notification,NotificationDto>(notif));
        }
    }
}
