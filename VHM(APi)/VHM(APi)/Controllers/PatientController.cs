using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VHM_APi.Helper;
using Talabat.BLL.Specification;
using Talabat.BLL.Specification.ProductSpecification;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi_.Dtos;
using VHW.BLL.Specification.PatientSpec;
using VHW.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM_APi_.Errors;
using VHM_APi_.Document;
using VHM_APi_.Dtos.Doctor;
using System.Numerics;
using VHM_APi_.EntityInputs;
using VHM_APi_.EntityInputs.StaticDataInputs;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM_APi_.Controllers
{

    public class PatientController : BaseApiController
    {
        private readonly IPatientRepository patientRepo;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISavedRepository savedRepository;
        private readonly IGenericRepository<Cardiovascular> cardioRepo;
        private readonly IGenericRepository<BrainStroke> brainRepo;
        private readonly IGenericRepository<KidneyFeatures> kidneyRepo;
        private readonly IGenericRepository<DiabetsFeatures> diabtesRepo;

        public PatientController(IPatientRepository patientRepo, IMapper mapper, UserManager<ApplicationUser> userManager
            , ISavedRepository savedRepository, IReadingRepository ReadingRepo,IGenericRepository<Cardiovascular> cardioRepo,
            IGenericRepository<BrainStroke> brainRepo,IGenericRepository<KidneyFeatures> kidneyRepo,IGenericRepository<DiabetsFeatures>diabtesRepo)
        {
            this.patientRepo = patientRepo;
            this.mapper = mapper;
            this.userManager = userManager;
            this.savedRepository = savedRepository;
            this.cardioRepo = cardioRepo;
            this.brainRepo = brainRepo;
            this.kidneyRepo = kidneyRepo;
            this.diabtesRepo = diabtesRepo;
        }
        [Authorize(Roles ="Doctor,Admin,Staff")] //bring back again nooooteeeee
        [HttpGet]
        public async Task<ActionResult<Pagination<PatientDto>>> GetAllPatient([FromQuery] PatientParams Params)
        {
            var specification = new PatientWithReportSpecification(Params);
            var result = await patientRepo.GetAllWithSpec(specification);
            var count = new PatientsWithFiltersForCountSpecification(Params);
            var map = mapper.Map<IReadOnlyList<Patient>, IReadOnlyList<PatientDto>>(result);
            var page = new Pagination<PatientDto>(Params.PageIndex, Params.PageSize, await patientRepo.GetCountAsync(count), map);
            return Ok(page);
        }


        [Authorize]
        [HttpGet("GetPatientById")]
        public async Task<ActionResult<PatientDto>> GetPatientById(string id = null)
        {
            string patientId;
            bool isPatient = User.IsInRole("Patient");

            if (isPatient)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                patientId = user.Id;

            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new ApiErrorResponse(400, "Patient ID cannot be null or empty."));
                }
                patientId = id;
            }
            var specification = new PatientWithReportSpecification(patientId);
            var patient = await patientRepo.GetByIdWithSpec(specification);
            if (patient == null) return NotFound(new ApiErrorResponse(404));
            var mapped = mapper.Map<Patient, PatientDto>(patient);
            return Ok(mapped);
        }


        [Authorize(Roles = "Patient")]
        [HttpPut("UpdatePatientProfile")]
        public async Task<ActionResult<ApplicationUserDto>> UpdatePatientProfile([FromForm] ApplicationUserInputDto patientDto)
        {
            var user = (Patient)await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user.Id != patientDto.Id) return BadRequest(new ApiErrorResponse(400, "Cannot update this profile"));
            string ImageUrl;
            if (patientDto.Image != null)
            {
                ImageUrl = DocumetSettings.UploadFile(patientDto.Image, "Imgs");
                user.ImageUrl = ImageUrl;
            }

            user.Age = patientDto.Age;
            user.Address = patientDto.Address;
            user.Email = patientDto.Email;
            user.PhoneNumber = patientDto.PhoneNumber;

            user.UserName = patientDto.UserName;
            var normalizedName = userManager.NormalizeName(patientDto.UserName);
            user.NormalizedUserName = normalizedName;



            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(mapper.Map<Patient, PatientDto>(user));
            return BadRequest(new ApiExceptionErrorResponse(400, result.Errors.ToString()));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deletePatient/{PatientId}")]
        public async Task<ActionResult<ApplicationUserDto>> DeletePatient(string PatientId)
        {
            var patient = (Patient)await userManager.FindByIdAsync(PatientId);
            if (patient == null) return NotFound(new ApiErrorResponse(400));
            if (patient.ImageUrl != null)
                DocumetSettings.DeleteFile(patient.ImageUrl);

            var result = await userManager.DeleteAsync(patient);
            if (result.Succeeded) return Ok(mapper.Map<Patient, PatientDto>(patient));
            return BadRequest(new ApiErrorResponse(400, result.Errors.ToString()));
        }



        [Authorize(Roles = "Doctor,Staff,Admin")]
        [HttpGet("AddToSaved")]
        public async Task<ActionResult> AddToSaved(string PatientId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var patient = await patientRepo.GetWithId(PatientId);
            if (patient != null)
            {
                var Saved = new UserWithSavedPatient()
                {
                    UserId = user.Id,
                    PatientId = patient.Id,
                };
                bool isSaved = await savedRepository.CheckIfPatientAlreadyAddedToSaved(user.Id, PatientId);
                if (!isSaved)
                    await savedRepository.Add(Saved);

                return Ok(mapper.Map<Patient, PatientDto>(patient));
            }
            return NotFound(new ApiErrorResponse(404));
        }



        [Authorize(Roles = "Doctor,Staff,Admin")]
        [HttpGet("RemoveFromSaved/{PatientId}")]
        public async Task<ActionResult> RemoveFromSaved(string PatientId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var patient = await patientRepo.GetWithId(PatientId);
            if (patient != null)
            {
                var Saved = new UserWithSavedPatient()
                {
                    UserId = user.Id,
                    PatientId = patient.Id,
                };
                bool isSaved = await savedRepository.CheckIfPatientAlreadyAddedToSaved(user.Id, PatientId);
                if (isSaved)
                    await savedRepository.Delete(Saved);
                return Ok(mapper.Map<Patient, PatientDto>(patient));
            }
            return NotFound(new ApiErrorResponse(404));
        }



        [Authorize(Roles = "Doctor,Staff,Admin")]
        [HttpGet("ViewSavedPatient")]
        public async Task<ActionResult<Pagination<PatientDto>>> ViewSavedPatient([FromQuery] PatientParams Params)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var specification = new PatientWithReportSpecification(Params);
            var result = await patientRepo.GetSavedPatientWithSpesc(user.Id, specification);
            var count = await patientRepo.GetCountForSavedPatientAsync(user.Id, specification);
            var map = mapper.Map<IReadOnlyList<Patient>, IReadOnlyList<PatientDto>>(result);
            var page = new Pagination<PatientDto>(Params.PageIndex, Params.PageSize, count, map);

            return Ok(page);
        }

        [Authorize]
        [HttpGet("GetResponsibleDoctors")]
        public async Task<ActionResult<IReadOnlyList<DoctorDto>>> GetResponsible(string PatientId = null)
        {
            string PId;
            bool isPatient = User.IsInRole("Patient");
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (isPatient)
            {
                PId = user.Id;
            }
            else
            {
                if (PatientId == null) return BadRequest(new ApiErrorResponse(400, "PatientId should be provided"));
                PId = PatientId;
            }
            var doctors = await patientRepo.GetResponsibleDoctors(PId);
            var mapped = mapper.Map<IReadOnlyList<DoctorDto>>(doctors);
            return Ok(mapped);
        }


        [Authorize(Roles = "Admin,Doctor,Staff")]
        [HttpPost("AddCadioData")]
        public async Task<ActionResult> AddCadioData(AddCardioData staticData)
        {
            var patient = (Patient)await userManager.FindByIdAsync(staticData.PatientId);
            if (patient == null) return BadRequest(new ApiErrorResponse(400));


            Cardiovascular diseaseFeatures;
            //
            //var d = (Cardiovascular)staticData.DiseaseFeatures;
            diseaseFeatures = new Cardiovascular()
            {
                Ap_hi = staticData.Ap_hi,
                Ap_lo = staticData.Ap_lo,
                Cholesterol = staticData.Cholesterol,
                Height = staticData.Height,
                Weight = staticData.Weight,
            };
            var type= await cardioRepo.Add(diseaseFeatures);
            patient.DiseaseFeaturesId = type.Id;
            //

            await patientRepo.Update(patient);
            return Ok(staticData);
        }

        [Authorize(Roles = "Admin,Doctor,Staff")]
        [HttpPost("AddKidneyData")]
        public async Task<ActionResult> AddKidneyData(AddKidneyData staticData)
        {
            var patient = (Patient)await userManager.FindByIdAsync(staticData.PatientId);
            if (patient == null) return BadRequest(new ApiErrorResponse(400));


            KidneyFeatures diseaseFeatures;
            diseaseFeatures = new KidneyFeatures()
            {
                Albumin = staticData.Albumin,
                BloodglucoseRandom = staticData.BloodglucoseRandom,
                BloodUrea = staticData.BloodUrea,
                Haemoglobin = staticData.Haemoglobin,
                Hypertension = staticData.Hypertension,
                PackedCellVolume = staticData.PackedCellVolume,
                SerumCreatinine = staticData.SerumCreatinine,
                Sugar = staticData.Sugar,
                whiteBloodCellCount = staticData.whiteBloodCellCount
            };
            var type = await kidneyRepo.Add(diseaseFeatures);
            patient.DiseaseFeaturesId = type.Id;
            await patientRepo.Update(patient);
            return Ok(staticData);
        }



        [Authorize(Roles = "Admin,Doctor,Staff")]
        [HttpPost("AddBrainStrokeData")]
        public async Task<ActionResult> AddBrainStrokeData(AddBrainStrokerData staticData)
        {
            var patient = (Patient)await userManager.FindByIdAsync(staticData.PatientId);
            if (patient == null) return BadRequest(new ApiErrorResponse(400));


            BrainStroke diseaseFeatures;
            diseaseFeatures = new BrainStroke()
            {
                BMI = staticData.BMI,
                AvgGlucose = staticData.AvgGlucose,
                HeartDisease = staticData.HeartDisease,
                Hypertension = staticData.Hypertension,
            };
            var type = await brainRepo.Add(diseaseFeatures);
            patient.DiseaseFeaturesId = type.Id;
            await patientRepo.Update(patient);
            return Ok(staticData);
        }




        [Authorize(Roles = "Admin,Doctor,Staff")]
        [HttpPost("AddDiabetesData")]
        public async Task<ActionResult> AddDiabetesData(AddDiabetesData staticData)
        {
            var patient = (Patient)await userManager.FindByIdAsync(staticData.PatientId);
            if (patient == null) return BadRequest(new ApiErrorResponse(400));


            DiabetsFeatures diseaseFeatures;
            diseaseFeatures = new DiabetsFeatures()
            {
                BMI = staticData.BMI,
                Glucose = staticData.Glucose,
                Insulin = staticData.Insulin,
                SkinThickness = staticData.SkinThickness,
            };
            var type = await diabtesRepo.Add(diseaseFeatures);
            patient.DiseaseFeaturesId = type.Id;
            await patientRepo.Update(patient);
            return Ok(staticData);
        }
    }
}