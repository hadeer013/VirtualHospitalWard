using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.Ambulance;
using VHM_APi.Helper;
using VHM_APi_.Dtos.Ambulance;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Repositories.AmbulanceRepository;
using VHW.BLL.Specification;
using VHW.BLL.Specification.AmbulanceSpec;

namespace VHM_APi_.Controllers
{
    public class AmbulanceController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAmbulanceRepository ambulanceCallRepo;
        private readonly IMapper mapper;

        public AmbulanceController(UserManager<ApplicationUser> userManager,IAmbulanceRepository ambulanceCallRepo,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.ambulanceCallRepo = ambulanceCallRepo;
            this.mapper = mapper;
        }

        [Authorize(Roles ="Patient")]
        [HttpPost("CallAmbulance")]
        public async Task<ActionResult<AmbulanceCallDto>> CallAmbulance(PatientLocation patientLocation)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var patient = await userManager.FindByEmailAsync(email);
            var ambulanceCall = new AmbulanceCall()
            {
                Location = patientLocation,
                PatientId = patient.Id,
            };
            var amb= await ambulanceCallRepo.Add(ambulanceCall);

            return Ok(new AmbulanceCallDto()
            {
                Id = amb.Id,
                Location = amb.Location,
                PatientId = amb.PatientId,
                PatientUserName = patient.UserName,
                Date = amb.Date,
                Status = RequestStatus.Pending
            });

        }

        [Authorize(Roles ="Admin,Staff")]
        [HttpGet]
        public async Task<ActionResult<Pagination<AmbulanceCallDto>>> GetAllAmbulanceCalls([FromQuery] BaseFilterationParams filterationParams,string PatientId=null)
        {
            if(PatientId!=null)
            {
                var patient = await userManager.FindByIdAsync(PatientId);
                if (patient == null) return BadRequest(new ApiErrorResponse(400));
            }
            var specification = new AmbulanceSpecification(filterationParams, PatientId);
            var Calls = await ambulanceCallRepo.GetAllWithSpec(specification);
            var filterSpec = new AmbulanceWithFilterForCountSpec(filterationParams, PatientId);
            var count = await ambulanceCallRepo.GetCountAsync(filterSpec);
            var data = mapper.Map<IReadOnlyList<AmbulanceCallDto>>(Calls);
            var pagination = new Pagination<AmbulanceCallDto>(filterationParams.PageIndex, filterationParams.PageSize, count,data);
            return Ok(pagination);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AmbulanceCallDto>> GetSpecificAmbulanceCall(int id)
        {
            var specification = new AmbulanceSpecification(id);
            var Calls = await ambulanceCallRepo.GetByIdWithSpec(specification);
            var data = mapper.Map<AmbulanceCallDto>(Calls);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("UpdateAmbulanceRequest/{CallId}")]
        public async Task<ActionResult> ChangeAmbulanceRequest(int CallId) 
        {
            var spec=new AmbulanceSpecification(CallId);
            var Call = await ambulanceCallRepo.GetByIdWithSpec(spec);
            if (Call == null) return BadRequest(new ApiErrorResponse(400));
            Call.Status = RequestStatus.Approved;
            await ambulanceCallRepo.Update(Call);
            return Ok(mapper.Map<AmbulanceCallDto>(Call));
        }


    }
}
