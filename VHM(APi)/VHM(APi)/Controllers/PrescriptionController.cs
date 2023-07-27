using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using VHW.BLL.Specification.PrescriptionSpec;


namespace VHM_APi_.Controllers
{
    
    public class PrescriptionController : BaseApiController
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPatientRepository patientRepo;
        private readonly IPrescriptionRepository prescriptionRepository;

        public PrescriptionController(IMapper mapper,UserManager<ApplicationUser> userManager, IPatientRepository PatientRepo,IPrescriptionRepository prescriptionRepository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            patientRepo = PatientRepo;
            this.prescriptionRepository = prescriptionRepository;
        }




        [Authorize(Roles = "Doctor")]
        [HttpPost("writePrescription")]
        public async Task<ActionResult<PrescriptionDto>> WritePrescription(PrescriptionInput prescription)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var patient = await patientRepo.GetWithId(prescription.PatientId);
            var Newprescription = new Prescription()
            {
                Content = prescription.Content,
                DoctorId = user.Id,
                PatientId = patient.Id
            };
            await prescriptionRepository.Add(Newprescription);
            var dto = new PrescriptionDto()
            {
                Id=Newprescription.Id,
                Content = Newprescription.Content,
                DoctorName = user.UserName,
                CreationDate = Newprescription.CreationDate
            };
            return Ok(dto);
        }//done






        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet("GetPatientPrescriptions")]
        public async Task<ActionResult<IReadOnlyList<PrescriptionDto>>> GetPrescriptionByPatientId([FromQuery] BaseFilterationParams PrescriptionParams, string id = null)
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
                if (id == null) return BadRequest(new ApiErrorResponse(400));
                patientId = id;
            }
            var spec = new PrescriptionSpecification(patientId, PrescriptionParams);
            var patient = await patientRepo.GetWithId(patientId);
            if (patient != null)
            {
                var result = await prescriptionRepository.GetAllWithSpec(spec);
                var mapped = mapper.Map<IReadOnlyList<Prescription>, IReadOnlyList<PrescriptionDto>>(result);
                var count = new PrescriptionWithFilterForCountSpec(patientId, PrescriptionParams);
                var page = new Pagination<PrescriptionDto>(PrescriptionParams.PageIndex, PrescriptionParams.PageSize, await prescriptionRepository.GetCountAsync(count), mapped);
                return Ok(page);
            }
            return NotFound(new ApiErrorResponse(404));
        }



        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet("prescription/{PrescriptionId}")]  //updated
        public async Task<ActionResult<PrescriptionDto>> GetSpecificPrescriptionByPrescriptionId(int PrescriptionId)
        {

            string patientId;
            bool isPatient = User.IsInRole("Patient");
            var spec = new PrescriptionSpecification(PrescriptionId);
            var prescription = await prescriptionRepository.GetByIdWithSpec(spec);
            if (isPatient)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                patientId = user.Id;
                if (!(prescription != null && prescription.Patient.Id == user.Id)) 
                    return BadRequest(new ApiErrorResponse(400));
            }

            if(prescription != null)
            {
                var mapped = mapper.Map<Prescription, PrescriptionDto>(prescription);
                return Ok(mapped);
            }
            return NotFound(new ApiErrorResponse(404));
        }

    }
}
