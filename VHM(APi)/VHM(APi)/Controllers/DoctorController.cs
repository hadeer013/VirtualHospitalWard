using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ConsultaTask;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi.Helper;
using VHM_APi_.Document;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.Doctor;
using VHM_APi_.EntityInputs.DoctorInputs;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification.DoctorSpec;
using VHW.BLL.Specification.PatientSpec;

namespace VHM_APi_.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDoctorRepository docRepo;
        private readonly IMapper mapper;

        public DoctorController(UserManager<ApplicationUser> userManager,IDoctorRepository DocRepo, IMapper mapper)
        {
            this.userManager = userManager;
            docRepo = DocRepo;
            this.mapper = mapper;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllDoctor([FromQuery] DoctorParams Params)  //search with name or department
        {
            var specification = new DoctorSpecification(Params);
            var result = await docRepo.GetAllWithSpec(specification);
            var count = new DoctorWithFilterForCountSpec(Params);
            var map = mapper.Map<IReadOnlyList<Doctor>, IReadOnlyList<DoctorDto>>(result);
            var page = new Pagination<DoctorDto>(Params.PageIndex, Params.PageSize, await docRepo.GetCountAsync(count), map);

            return Ok(page);
        }


        [Authorize]
        [HttpGet("GetDoctor")]
        public async Task<ActionResult<DoctorDto>> GetDoctorById(string id = null)
        {
            string doctorId;
            bool isDoctor= User.IsInRole("Doctor");

            if (isDoctor)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                doctorId = user.Id;
                
            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new ApiErrorResponse(400, "Doctor ID cannot be null or empty."));
                }
                doctorId = id;
            }

           //var specification = new DoctorSpecification(id);
            var Doctor = await docRepo.GetWithId(doctorId);
            if (Doctor == null) return NotFound(new ApiErrorResponse(404));
            var mapped = mapper.Map<Doctor, DoctorDto>(Doctor);
            return Ok(mapped);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("updateDoctorProfile")]
        public async Task<ActionResult<DoctorDto>> UpdateDoctorProfile([FromForm]UpdateDoctorInputDto doctorDto)
        {
            var user =(Doctor) await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user.Id != doctorDto.Id) return BadRequest(new ApiErrorResponse(400, "Cannot update this profile"));
            string ImageUrl;
            if (doctorDto.Image != null)
            {
                ImageUrl = DocumetSettings.UploadFile(doctorDto.Image, "Imgs");
                user.ImageUrl = ImageUrl;
            }

            user.Age = doctorDto.Age;
            user.Address = doctorDto.Address;
            user.Email = doctorDto.Email;
            user.PhoneNumber = doctorDto.PhoneNumber;

            user.UserName = doctorDto.UserName;
            var normalizedName = userManager.NormalizeName(doctorDto.UserName);
            user.NormalizedUserName = normalizedName;
            user.Department=doctorDto.Department;


            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {

                return Ok(mapper.Map<Doctor,DoctorDto>(user));
            }
               
            return BadRequest(new ApiExceptionErrorResponse(400, result.Errors.ToString()));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteDoctor/{doctorId}")]
        public async Task<ActionResult<DoctorDto>> DeleteDoctor(string doctorId)
        {
            var doctor = (Doctor)await userManager.FindByIdAsync(doctorId);
            if (doctor == null) return NotFound(new ApiErrorResponse(400));
            if (doctor.ImageUrl != null)
                DocumetSettings.DeleteFile(doctor.ImageUrl);

            var result = await userManager.DeleteAsync(doctor);
            if (result.Succeeded) return Ok(mapper.Map<Doctor, DoctorDto>(doctor));
            return BadRequest(new ApiErrorResponse(400, result.Errors.ToString()));
        }

        [Authorize(Roles ="Admin,Patient,Staff")]
        [HttpGet("getOnlineDoctors")]
        public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetOnlineDoctor()
        {
            var doctors=await docRepo.GetOnlineDoctor();
            var mapped = mapper.Map<IReadOnlyList<DoctorDto>>(doctors);
            return Ok(mapped);
        }






    }
}
