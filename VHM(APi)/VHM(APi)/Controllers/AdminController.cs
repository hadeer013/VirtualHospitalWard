using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM_APi.Helper;
using VHM_APi_.Document;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.Doctor;
using VHM_APi_.EntityInputs;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;
using VHW.BLL.Specification.AdminSpec;
using VHW.BLL.Specification.DoctorSpec;

namespace VHM_APi_.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository<Admin> adminRepo;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AdminController(UserManager<ApplicationUser> userManager, IUserRepository<Admin> adminRepo, IMapper mapper,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.adminRepo = adminRepo;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ApplicationUserDto>>> GetAllAdmins([FromQuery] BaseFilterationParams Params)  //search with name 
        {
            var specification = new AdminSpecification(Params);
            var result = await adminRepo.GetAllWithSpec(specification);
            var count = new AdminWithFilterForCount(Params);
            var map = mapper.Map<IReadOnlyList<Admin>, IReadOnlyList<ApplicationUserDto>>(result);
            var page = new Pagination<ApplicationUserDto>(Params.PageIndex, Params.PageSize, await adminRepo.GetCountAsync(count), map);

            return Ok(page);
        }


        [Authorize]
        [HttpGet("GetAdminById")]
        public async Task<ActionResult<ApplicationUserDto>> GetAdminById(string id = null)
        {
            string adminId;
            bool isAdmin = User.IsInRole("Admin");

            if (isAdmin)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                adminId = user.Id;

            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new ApiErrorResponse(400, "Admin ID cannot be null or empty."));
                }
                adminId = id;
            }

            var admin = await adminRepo.GetWithId(adminId);
            if (admin == null) return NotFound(new ApiErrorResponse(404));
            var mapped = mapper.Map<Admin, ApplicationUserDto>(admin);
            return Ok(mapped);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updateAdminProfile")]
        public async Task<ActionResult<ApplicationUserDto>> UpdateAdminProfile([FromForm] ApplicationUserInputDto adminDto)
        {
            var user = (Admin)await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user.Id != adminDto.Id) return BadRequest(new ApiErrorResponse(400, "Cannot update this profile"));

            string ImageUrl;
            if (adminDto.Image != null) {
                ImageUrl = DocumetSettings.UploadFile(adminDto.Image, "Imgs");
                user.ImageUrl = ImageUrl;/*$"files/Imgs/{ImageUrl}";*/
            }
               
            user.Age = adminDto.Age;
            user.Address = adminDto.Address;
            user.Email = adminDto.Email;
            user.PhoneNumber = adminDto.PhoneNumber;
           
            user.UserName = adminDto.UserName;
            var normalizedName = userManager.NormalizeName(adminDto.UserName);
            user.NormalizedUserName= normalizedName;



            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(/*mapper.Map<ApplicationUserDto>(user)*/
                    new ApplicationUserDto()
                    {
                        Id = user.Id,
                        Age = user.Age,
                        Email = user.Email,
                        ImageUrl = string.IsNullOrEmpty(user.ImageUrl)? null:$"{configuration["BaseUrl"]}{user.ImageUrl}",
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        Gender = user.Gender.ToString(),
                    });
            }
                
            return BadRequest(new ApiExceptionErrorResponse(400,result.Errors.ToString()));
           
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteAdmin/{adminId}")]
        public async Task<ActionResult<ApplicationUserDto>> DeleteAdmin(/*[FromForm]ApplicationUserInputDto*/ string adminId)
        {
            //var MappedAdmin = mapper.Map<ApplicationUserDto, Admin>(adminDto);
            var admin =(Admin) await userManager.FindByIdAsync(adminId);
            
            if (admin == null) return NotFound(new ApiErrorResponse(400));
            if (admin.ImageUrl != null)
                DocumetSettings.DeleteFile(admin.ImageUrl);

            var result = await userManager.DeleteAsync(admin);
            if (result.Succeeded) return Ok(mapper.Map<Admin, ApplicationUserDto >(admin));
            return BadRequest(new ApiErrorResponse(400, result.Errors.ToString()));
        }
    }
}
