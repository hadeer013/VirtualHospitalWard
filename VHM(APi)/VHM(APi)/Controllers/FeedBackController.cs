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
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;
using VHW.BLL.Specification.FeedBackSpec;

namespace VHM_APi_.Controllers
{
    public class FeedBackController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFeedBackRepository feedBackRepository;
        private readonly IMapper mapper;

        public FeedBackController(UserManager<ApplicationUser> userManager,IFeedBackRepository feedBackRepository,IMapper mapper)
        {
            this.userManager = userManager;
            this.feedBackRepository = feedBackRepository;
            this.mapper = mapper;
        }


        [Authorize(Roles = "Patient")]
        [HttpPost("WriteFeedback")]
        public async Task<ActionResult<FeedBack>> WriteFeedBack(FeedBackInput feedBack)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = (Patient)await userManager.FindByEmailAsync(email);

            var Feed = new FeedBack()
            {
                content = feedBack.content,
                RateValue = feedBack.RateValue,
                PatientId = user.Id
            };
            await feedBackRepository.Add(Feed);
            return Ok(new FeedBackDto()
            {
                Id = Feed.Id,
                content = feedBack.content,
                RateValue = feedBack.RateValue,
                PatientName = user.UserName,
                CreationDate = Feed.CreationDate
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllFeedbacks")]
        public async Task<ActionResult> GetAllFeedBacks([FromQuery] FeedBackParams feedBackParams)
        {
            var specification = new FeedBackSpecification(feedBackParams);
            var feedBacks = await feedBackRepository.GetAllWithSpec(specification);
            var count = new FeedBackWithFilterWithSpec(feedBackParams);
            var result = mapper.Map<IReadOnlyList<FeedBack>, IReadOnlyList<FeedBackDto>>(feedBacks);
            var page = new Pagination<FeedBackDto>(feedBackParams.PageIndex,
                feedBackParams.PageSize, await feedBackRepository.GetCountAsync(count), result);
            return Ok(page);
        }


        [Authorize(Roles ="Admin")]
        [HttpGet("GetFeedBackById/{FeedBackId}")]
        public async Task<ActionResult> GetFeedBackById(int FeedBackId)
        {
            var spec = new FeedBackSpecification(FeedBackId);
            var feed= await feedBackRepository.GetByIdWithSpec(spec);
            if(feed!=null)
            {
                var result = mapper.Map<FeedBack, FeedBackDto>(feed);
                return Ok(result);
            }
            return NotFound(new ApiErrorResponse(404));
        }
    }
}
