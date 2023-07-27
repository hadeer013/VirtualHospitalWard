using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi_.Dtos;
using VHM_APi_.EntityInputs;
using VHM_APi_.EntityInputs.DoctorInputs;
using VHM_APi_.Errors;
using VHM_APi_.Helper.Email;
using VHM_APi_.Helper.Password_generator;
using VHW.BLL.Interfaces;


namespace VHM_APi_.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ITokenService tokenService,IMapper mapper,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        //Login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if(user!=null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (result.Succeeded)
                {
                    var token = await tokenService.CreateToken(user, userManager);
                    return new UserDto()
                    { 
                        UserName = user.UserName,
                        Email = loginDto.Email,
                        Token = token
                    };
                }
                    
            }
            return Unauthorized(new ApiErrorResponse(401));
        }


        //Add New Doctor
        [Authorize(Roles ="Admin")]
        [HttpPost("AddDoctor")]
        public async Task<ActionResult<UserDto>> AddDoctor(AddDoctorDto doctorInput)
        {

            var doctor = new Doctor()
            {
                Age = doctorInput.Age,
                Email = doctorInput.Email,
                Gender = (Gender)Enum.Parse(typeof(Gender), doctorInput.Gender, true),
                Department = doctorInput.Department,
                PhoneNumber = doctorInput.PhoneNumber,
                UserName = doctorInput.UserName,
                Address = doctorInput.Address
            };
            if (CheckEmailExists(doctorInput.Email).Result.Value)
                return BadRequest();
            var password = PasswordGeneration.Generator();
            var result = await userManager.CreateAsync(doctor, password);
            if (result.Succeeded)
            {  
                var email = new Email()
                {
                    Subject = "Virtual Hospital Word < Password Generation >",
                    Body = $"Please use this password in login process : {password}",
                    To = doctorInput.Email
                };
                EmailSettings.SendEmail(email);
                var role = await roleManager.FindByNameAsync("Doctor");
                var added = await userManager.AddToRoleAsync(doctor, role.Name);
                if (added.Succeeded)
                {
                    var user = new BaseUserDto()
                    {
                       
                        Email = doctor.Email,
                        UserName = doctor.UserName
                         
                    };
                    return Ok(user);
                }
            }
            return BadRequest(result.Errors);

        }

        //Add Patient
        //[Authorize(Roles = "Admin")]
        [HttpPost("AddPatient")]
        public async Task<ActionResult<UserDto>> AddPatient(PatientInput patientInput)
        {
            #region Old
            //map patientInput to Patient and add to database
            //var mapped = mapper.Map<PatientInput, Patient>(patient);

            //await patientRepository.AddUser(mapped);
            ////make VHWId == Patient.ID

            ////var appUser = new ApplicationUser()
            ////{

            ////    Name = patient.Name,
            ////    Email = patient.Email,
            ////    PhoneNumber = patient.PhoneNumber,
            ////    UserName = patient.Email.Split('@')[0],
            ////};
            ////await patientRepository.AddUser(mapped);
            ////add appUser to database
            ////add role to user
            //var result = await userManager.CreateAsync(mapped, patient.Password);
            //if (result.Succeeded)
            //{
            //    mapped.
            //    var role = await roleManager.FindByNameAsync("Patient");
            //    await userManager.AddToRoleAsync(mapped, role.Name);
            //    //send sms to patient's phone
            //    var token = await tokenService.CreateToken(mapped, userManager);
            //    return Ok(new UserDto()
            //    {
            //        UserName = mapped.UserName,
            //        Email = mapped.Email,
            //        Token = token
            //    });
            //}

            //return BadRequest(result.Errors);
            #endregion 
            var patient = new Patient()
            {
                Age = patientInput.Age,
                Email = patientInput.Email,
                Gender = patientInput.Gender,
                Disease = patientInput.Disease,
                PhoneNumber = patientInput.PhoneNumber,
                UserName = patientInput.UserName
            };

            if (CheckEmailExists(patientInput.Email).Result.Value)
                return BadRequest();
            var password = PasswordGeneration.Generator();
            var result = await userManager.CreateAsync(patient, password);
            if (result.Succeeded)
            {
                var email = new Email()
                {
                    Subject = "Virtual Hospital Word < Password Generation >",
                    Body = $"Please use this password in login process : {password}",
                    To = patientInput.Email
                };
                EmailSettings.SendEmail(email);
                var role = await roleManager.FindByNameAsync("Patient");
                var added = await userManager.AddToRoleAsync(patient, role.Name);
                if (added.Succeeded)
                {
                    var user = new BaseUserDto()
                    {
                        Email = patient.Email,
                        UserName = patient.UserName
                    };
                    return Ok(user);
                }
            }
            return BadRequest(result.Errors);
        }

        

        //Add Admin
        //[Authorize(Roles = "Admin")]
        [HttpPost("AddAdmin")]
        public async Task<ActionResult<UserDto>> AddAdmin(AdminStaffAdditionInput adminStaffAddition)
        {
            
            var admin = new Admin()
            {
                Age = adminStaffAddition.Age,
                Email = adminStaffAddition.Email,
                Gender = (Gender)Enum.Parse(typeof(Gender), adminStaffAddition.Gender, true),
                PhoneNumber = adminStaffAddition.PhoneNumber,
                UserName = adminStaffAddition.UserName
            };
            if (CheckEmailExists(adminStaffAddition.Email).Result.Value)
                return BadRequest();
            var password = PasswordGeneration.Generator();
            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                var email = new Email()
                {
                    Subject = "Virtual Hospital Word < Password Generation >",
                    Body = $"Please use this password in login process : {password}",
                    To = adminStaffAddition.Email
                };
                EmailSettings.SendEmail(email);
                var role = await roleManager.FindByNameAsync("Admin");
                var added = await userManager.AddToRoleAsync(admin, role.Name);
                if (added.Succeeded)
                {
                    var user = new BaseUserDto()
                    {
                        Email = admin.Email,
                        UserName = admin.UserName
                    };
                    return Ok(user);
                }
            }
            return BadRequest(result.Errors);
        }

        //Add Staff
        [Authorize(Roles = "Admin")]
        [HttpPost("AddStaff")]
        public async Task<ActionResult<UserDto>> AddStaff(AdminStaffAdditionInput adminStaffAddition)
        {

            var staff = new Staff()
            {
                Age = adminStaffAddition.Age,
                Email = adminStaffAddition.Email,
                Gender = (Gender)Enum.Parse(typeof(Gender), adminStaffAddition.Gender, true),
                PhoneNumber = adminStaffAddition.PhoneNumber,
                UserName = adminStaffAddition.UserName
            };
            if (CheckEmailExists(adminStaffAddition.Email).Result.Value)
                return BadRequest();
            var password = PasswordGeneration.Generator(); 
            var result = await userManager.CreateAsync(staff, password);
            if (result.Succeeded)
            {
                var email = new Email()
                {
                    Subject = "Virtual Hospital Word < Password Generation >",
                    Body = $"Please use this password in login process : {password}",
                    To = adminStaffAddition.Email
                };
                EmailSettings.SendEmail(email);
                var role = await roleManager.FindByNameAsync("Staff");
                var added = await userManager.AddToRoleAsync(staff, role.Name);
                if (added.Succeeded)
                {
                    var user = new BaseUserDto()
                    {
                        Email = staff.Email,
                        UserName = staff.UserName
                    };
                    return Ok(user);
                }
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("GetUserByName")]
        public async Task<ActionResult> GetUserByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest(new ApiErrorResponse(400));
            var user=await userManager.FindByNameAsync(name);
            if (user == null) return NotFound(new ApiErrorResponse(404));
            var mapped = mapper.Map<ApplicationUser, BaseUserDto>(user);
            return Ok(mapped);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery]string Email)
        {
           return await userManager.FindByEmailAsync(Email) != null;
        }

    }
}
