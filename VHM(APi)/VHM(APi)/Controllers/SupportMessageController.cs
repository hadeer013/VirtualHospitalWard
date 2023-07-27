using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi.Helper;
using VHM_APi_.Dtos;
using VHM_APi_.EntityInputs;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;
using VHW.BLL.Specification.SupportMessagesSpec;

namespace VHM_APi_.Controllers
{
    public class SupportMessageController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISupportMessageRepository supportRepo;
        private readonly IMapper mapper;

        public SupportMessageController(UserManager<ApplicationUser> userManager,ISupportMessageRepository supportRepo,IMapper mapper)
        {
            this.userManager = userManager;
            this.supportRepo = supportRepo;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<ActionResult<SupportMessageDto>> WriteSupportMessage(SupportMessageInput supportMessages)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var patient = await userManager.FindByEmailAsync(email);
            var message = new SupportMessages()
            {
                Content = supportMessages.Content,
                PatientId = patient.Id
            };
            await supportRepo.Add(message);
            return Ok(new SupportMessageDto()
            {
                Id = message.Id,
                Content = supportMessages.Content,
                CreationDate = message.CreationDate,
                PatientName = patient.UserName
            });
        }


        [Authorize(Roles ="Admin,Staff")]
        [HttpGet]
        public async Task<ActionResult<Pagination<SupportMessageDto>>> GetAllSupportMessages([FromQuery]SupportMessParams Params)
        {
            var spec = new SupportMessagesSpecification(Params);
            var result = await supportRepo.GetAllWithSpec(spec);

            var count = new SupportMessagesWithFilterWithSpec(Params);
            var map = mapper.Map<IReadOnlyList<SupportMessages>, IReadOnlyList<SupportMessageDto>>(result);
            var page = new Pagination<SupportMessageDto>(Params.PageIndex, Params.PageSize, await supportRepo.GetCountAsync(count), map);

            return Ok(page);
        }

        [Authorize(Roles ="Admin,Staff")]
        [HttpGet("GetSupportMessageById/{Id}")]
        public async Task<ActionResult<SupportMessageDto>> GetSupportMessageById(int Id)
        {
            var spec = new SupportMessagesSpecification(Id);
            var Message = await supportRepo.GetByIdWithSpec(spec);
            var Map = mapper.Map<SupportMessages, SupportMessageDto>(Message);
            return Ok(Map);
        }
    }
}
