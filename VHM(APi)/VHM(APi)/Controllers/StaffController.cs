using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi.Helper;
using VHM_APi_.Document;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.Doctor;
using VHM_APi_.EntityInputs;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification.DoctorSpec;
using VHW.BLL.Specification.PatientSpec;
using VHW.BLL.Specification.StaffSpec;

namespace VHM_APi_.Controllers
{

    public class StaffController : BaseApiController
    {
        private readonly IUserRepository<Staff> staffRepo;
        private readonly IMapper mapper;
        private readonly IUserRepository<Admin> adminRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public StaffController( IUserRepository<Staff> staffRepo,IMapper mapper,IUserRepository<Admin> adminRepo,UserManager<ApplicationUser> userManager)
        {
            this.staffRepo = staffRepo;
            this.mapper = mapper;
            this.adminRepo = adminRepo;
            this.userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllStaffs([FromQuery] PatientParams Params)
        {
            var specification = new StaffSpec(Params);
            var result = await staffRepo.GetAllWithSpec(specification);
            var count = new StaffWithFilterForSpec(Params);
            var map = mapper.Map<IReadOnlyList<Staff>, IReadOnlyList<ApplicationUserDto>>(result);
            var page = new Pagination<ApplicationUserDto>(Params.PageIndex, Params.PageSize, await staffRepo.GetCountAsync(count), map);

            return Ok(page);
        }


        [Authorize(Roles = "Admin")]      ///For test
        [HttpGet("admins")]
        public async Task<ActionResult> GetAllAdmins()
        {
            // var specification = new StaffSpec(Params);
            var result = await adminRepo.GetAll();
            //var count = new StaffWithFilterForSpec(Params);
            //var map = mapper.Map<IReadOnlyList<Staff>, IReadOnlyList<StaffDto>>(result);
            // var page = new Pagination<StaffDto>(Params.PageIndex, Params.PageSize, await staffRepo.GetCountAsync(count), map);

            return Ok(result);
        }


        [Authorize]
        [HttpGet("GetStaffById")]
        public async Task<ActionResult<ApplicationUserDto>> GetStaffById(string id=null)
        {
            string StaffId;
            bool isAdmin = User.IsInRole("Admin");

            if (isAdmin)
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new ApiErrorResponse(400, "Staff ID cannot be null or empty."));
                }
                StaffId = id;
            }
            else
            { 
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                StaffId = user.Id;
            }

            var specification = new StaffSpec(StaffId);
            var staff = await staffRepo.GetByIdWithSpec(specification);
            if (staff == null) return NotFound(new ApiErrorResponse(400,"There is no Staff member with the specified Id"));
            var mapped = mapper.Map<Staff, ApplicationUserDto>(staff);
            return Ok(mapped);
        }  //not tested yet


        [Authorize(Roles ="Staff")]
        [HttpPut("updateStaffProfile")]
        public async Task<ActionResult <ApplicationUserDto>>UpdateStaffProfile([FromForm] ApplicationUserInputDto staffDto)
        {
            var user =(Staff) await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user.Id != staffDto.Id) return BadRequest(new ApiErrorResponse(400, "Cannot update this profile"));
            string ImageUrl;
            if (staffDto.Image != null)
            {   
                ImageUrl = DocumetSettings.UploadFile(staffDto.Image, "Imgs");
                user.ImageUrl = ImageUrl;
            }

            user.Age = staffDto.Age;
            user.Address = staffDto.Address;
            user.Email = staffDto.Email;
            user.PhoneNumber = staffDto.PhoneNumber;

            user.UserName = staffDto.UserName;
            var normalizedName = userManager.NormalizeName(staffDto.UserName);
            user.NormalizedUserName = normalizedName;



            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(mapper.Map<Staff,ApplicationUserDto>(user));
            return BadRequest(new ApiExceptionErrorResponse(400, result.Errors.ToString()));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteStaff")]
        public async Task<ActionResult<ApplicationUserDto>> DeleteStaff(string staffId)
        {
            var staff = (Staff)await userManager.FindByIdAsync(staffId);
            if (staff == null) return NotFound(new ApiErrorResponse(400));
            if (staff.ImageUrl != null)
                DocumetSettings.DeleteFile(staff.ImageUrl);

            var result = await userManager.DeleteAsync(staff);
            if (result.Succeeded) return Ok(mapper.Map<Staff, ApplicationUserDto>(staff));
            return BadRequest(new ApiErrorResponse(400, result.Errors.ToString()));
        }
    }
}
