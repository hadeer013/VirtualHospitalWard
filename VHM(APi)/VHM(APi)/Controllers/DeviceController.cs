using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DevicesEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi_.Dtos.Devic;
using VHM_APi_.Errors;
using VHW.BLL.Interfaces;

namespace VHM_APi_.Controllers
{
    public class DeviceController : BaseApiController
    {
        private readonly IGenericRepository<Device> deviceRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public DeviceController(IGenericRepository<Device> deviceRepo, UserManager<ApplicationUser> userManager)
        {
            this.deviceRepo = deviceRepo;
            this.userManager = userManager;
        }

        //[Authorize(Roles ="Admin,Staff")]
        [HttpGet]
        public async Task<ActionResult> AddDevice(string PatientId=null)
        {
            string PId=null;
            if (PatientId != null)
            {
                var p=await userManager.FindByIdAsync(PatientId);

                if (p == null) return BadRequest(new ApiErrorResponse(400));
                PId = p.Id;
            }

            var device = new Device() { PatientId=PId};
            await deviceRepo.Add(device);
            return Ok(device);
        }

        //[Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult> AssignPatientToDevice(PatientDeviceAssignmentDto deviceAssignmentDto )
        {
            if (deviceAssignmentDto.PatientId == null) return BadRequest(new ApiErrorResponse(400));
            var patient=await userManager.FindByIdAsync(deviceAssignmentDto.PatientId);
            if (patient == null) return NotFound(new ApiErrorResponse(404));     
            
            var device = await deviceRepo.GetWithId(deviceAssignmentDto.DeviceId);
            if (device == null) return NotFound(new ApiErrorResponse(404));

            device.PatientId = patient.Id;
            await deviceRepo.Update(device);
            return Ok(new DeviceDto
            {
                DeviceId = device.Id,
                DeviceName = device.DeviceName,
                PatientId = patient.Id,
                PatientName = patient.UserName
            });
        }

    }
}
