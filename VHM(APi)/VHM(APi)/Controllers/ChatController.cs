using AutoMapper;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.Chat;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;

namespace VHM_APi_.Controllers
{
    public class ChatController : BaseApiController
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IChatRepository chatRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IConfiguration configuration;

        public ChatController(UserManager<ApplicationUser> userManager, IMapper mapper,
            IChatRepository chatRepository, IPatientRepository patientRepository,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.chatRepository = chatRepository;
            this.patientRepository = patientRepository;
            this.configuration = configuration;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LatestUserWithUnreadCountDto>>> GetLatestUsersWithUnreadCount()
        {
            var currentUser = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var GetLatestUsersWithUnreadCount = await chatRepository.GetLatestUsersWithUnreadCount(currentUser);
            IEnumerable<LatestUserWithUnreadCountDto> result = new List<LatestUserWithUnreadCountDto>();
            foreach (var i in GetLatestUsersWithUnreadCount)
            {
                var userData = await userManager.FindByIdAsync(i.UserId);
                //var mapped = mapper.Map<ApplicationUser, LatestUserWithUnreadCountDto>(userData);
                var LatestUserDto = new LatestUserWithUnreadCountDto()
                {
                    UserId = i.UserId,
                    UserName = userData.UserName,
                    ImageUrl = string.IsNullOrEmpty(userData.ImageUrl) ? null : $"{configuration["BaseUrl"]}{userData.ImageUrl}",
                    UnreadCount = i.UnreadCount,
                };
                LatestUserDto.Role = (await userManager.GetRolesAsync(userData)).FirstOrDefault();
                result = result.Append(LatestUserDto);
            }

            return Ok(result);
        }


        [Authorize]
        [HttpGet("NewChatUsers")]
        public async Task<IActionResult> GetNewUnChattedUsers()
        {
            IReadOnlyList<string> chatUsersId = new List<string>();
            IReadOnlyList<ApplicationUser> chatUsers = new List<ApplicationUser>();
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            bool isPatient = User.IsInRole("Patient");
            if (isPatient)
            {
                chatUsersId = await chatRepository.GetNewUsersIdForPatient(user);
            }
            else
            {
                chatUsersId = await chatRepository.GetNewUsersId(user);
            }
            chatUsers = await chatRepository.GetNewUsersForMessaging(chatUsersId);


            IEnumerable<LatestUserWithUnreadCountDto> result = new List<LatestUserWithUnreadCountDto>();
            foreach (var i in chatUsers)
            {
                // var mapped = mapper.Map<ApplicationUser, LatestUserWithUnreadCountDto>(i);
                var rtn = new LatestUserWithUnreadCountDto()
                {
                    UserId = i.Id,
                    UserName = i.UserName,
                    Role = (await userManager.GetRolesAsync(i)).FirstOrDefault(),
                    ImageUrl = string.IsNullOrEmpty(i.ImageUrl) ? null : $"{configuration["BaseUrl"]}{i.ImageUrl}",
                    UnreadCount = 0
                };
                result = result.Append(rtn);
            }

            return Ok(result);
        }



        [Authorize]
        [HttpGet("ChatPrivate")]
        public async Task<ActionResult> ChatPrivate(string ReceiverId)
        {
            if (ReceiverId == null) return BadRequest(new ApiErrorResponse(400));
            //if (ReceiverId == null) ReceiverId="13";
            var CurrentUser = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var reciever = await userManager.FindByIdAsync(ReceiverId);
            if (reciever != null)
            {
                var msgs = await chatRepository.GetMessages(CurrentUser.Id, reciever.Id);
                //await chatRepository.UpdateMessagesToRead(CurrentUser.Id, reciever.Id);
                //var MappedMegs = mapper.Map<IEnumerable<MessageDto>>(msgs);
                IEnumerable<MessageDto> result = new List<MessageDto>();
                foreach (var message in msgs)
                {
                    var res = new MessageDto()
                    {
                        Id = message.Id,
                        Contetnt = message.Contetnt,
                        SenderName = message.SenderUser.UserName,
                        RecieverName = message.ReceiverUser.UserName,
                        date = message.date,
                        IsSent = CurrentUser.UserName == message.SenderUser.UserName ? true : false
                    };
                    result = result.Append(res);
                }
                return Ok(result);
            }
            return NotFound(new ApiErrorResponse(404));
        }

    }

}