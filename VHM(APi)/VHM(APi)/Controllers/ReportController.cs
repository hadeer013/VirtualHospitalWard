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
using VHM_APi_.Dtos;
using VHM_APi_.EntityInputs;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;
using VHW.BLL.Repositories;
using VHW.BLL.Specification;
using VHW.BLL.Specification.PrescriptionSpec;
using VHW.BLL.Specification.ReportSpec;

namespace VHM_APi_.Controllers
{
    public class ReportController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReportRepository reportRepo;
        private readonly IPatientRepository patientRepo;
        private readonly IMapper mapper;

        public ReportController(UserManager<ApplicationUser> userManager,IReportRepository reportRepo,IPatientRepository patientRepo,IMapper mapper)
        {
            this.userManager = userManager;
            this.reportRepo = reportRepo;
            this.patientRepo = patientRepo;
            this.mapper = mapper;
        }
        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet("GetAllReports")]
        public async Task<ActionResult<Pagination<ReportDto>>> GetReportsByPatientId([FromQuery] BaseFilterationParams reportParams, string id = null)
        {
            string patientId;
            bool isPatient = User.IsInRole("Patient");
            if (isPatient)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                patientId= user.Id;
            }
            else
            {
                if (id == null) return BadRequest(new ApiErrorResponse(400));
                patientId = id;
            }
            var spec = new ReportSpecification(patientId, reportParams);
            var patient = await patientRepo.GetWithId(patientId);
            if (patient != null)
            {
                var result = await reportRepo.GetAllWithSpec(spec);
                var mapped = mapper.Map<IReadOnlyList<Report>, IReadOnlyList<ReportDto>>(result);
                var count = new ReportWithFilterForCountSpec(patientId, reportParams);
                var page = new Pagination<ReportDto>(reportParams.PageIndex, reportParams.PageSize,await reportRepo.GetCountAsync(count), mapped); 
                return Ok(page);
            }
            return NotFound(new ApiErrorResponse(404));
        }

        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet("report/{ReportId}")]
        public async Task<ActionResult<ReportDto>> GetSpecificReportByPatientId(int ReportId)
        {
            string patientId;
            bool isPatient = User.IsInRole("Patient");
            var spec = new ReportSpecification(ReportId);
            var report = await reportRepo.GetByIdWithSpec(spec);
            if (isPatient)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);
                patientId = user.Id;
                if (!(report != null && report.Patient.Id == user.Id))
                    return BadRequest(new ApiErrorResponse(400));
            }
            if(report != null)
            {
                var mapped = mapper.Map<Report, ReportDto>(report);
                return Ok(mapped);
            }
            return NotFound(new ApiErrorResponse(404));
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("writereport")]
        public async Task<ActionResult<ReportDto>> WriteReport(ReportInput report)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var patient = await patientRepo.GetWithId(report.PatientId);
            if (patient != null)
            {
                var NewReport = new Report()
                {
                    Content = report.Content,
                    DoctorId = user.Id,
                    PatientId = patient.Id
                };
                await reportRepo.Add(NewReport);

                var dto = new ReportDto()
                {
                    Id = NewReport.Id,
                    Content = NewReport.Content,
                    DoctorName = user.UserName,
                    CreationDate = NewReport.CreationDate
                };
                return Ok(dto);
            }
            return NotFound(new ApiErrorResponse(404)); 
        }

    }
}
