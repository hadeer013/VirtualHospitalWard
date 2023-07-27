using AutoMapper;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.DevicesEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.VideoCall;
using VHM_APi_.EntityInputs.StaticDataInputs;
using VHM_APi_.Errors;
using VHM_APi_.Helper;
using VHM_APi_.Hubs;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification.DeviceSpec;

namespace VHM_APi_.Controllers
{
    public class ReadingsController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReadingRepository readingRepo;
        private readonly IGenericRepository<Device> deviceRepo;
        private readonly IPatientRepository patientRepository;
        private readonly IHubContext<ReadingHub> readingContext;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly IUserWithConnectionIdsRepository connectionIdsRepository;
        private readonly IGenericRepository<DiseaseFeatures> diseaseFeatureRepo;

        public ReadingsController(UserManager<ApplicationUser> userManager,IReadingRepository ReadingRepo,
            IGenericRepository<Device> deviceRepo,IPatientRepository patientRepository, IHubContext<ReadingHub> ReadingContext
            ,IHubContext<NotificationHub> NotificationHub, IUserWithConnectionIdsRepository connectionIdsRepository,
            IGenericRepository<DiseaseFeatures> diseaseFeatureRepo)
        {
            this.userManager = userManager;
            readingRepo = ReadingRepo;
            this.deviceRepo = deviceRepo;
            this.patientRepository = patientRepository;
            readingContext = ReadingContext;
            notificationHub = NotificationHub;
            this.connectionIdsRepository = connectionIdsRepository;
            this.diseaseFeatureRepo = diseaseFeatureRepo;
        }

        [HttpPost] //http://www.cur.somee.com/api/Readings
        public async Task<ActionResult> GetNewPatientReadings(AddReadingsDto readingsDto)
        {
            var spec = new DeviceSpecification(readingsDto.DeviceId);
            var device = await deviceRepo.GetByIdWithSpec(spec);
            if (device == null) return Unauthorized(new ApiErrorResponse(401));
            if (device.PatientId == null) return BadRequest(new ApiErrorResponse(400));

            var patient = (Patient)await userManager.FindByIdAsync(device.PatientId);
            if (patient == null) return BadRequest(new ApiErrorResponse(400));
            
            var patientDiseaseFeature = await diseaseFeatureRepo.GetWithId(patient.DiseaseFeaturesId);
            
                
            
            DiseaseFeatures diseaseFeatures;

            if (patient.Disease == Disease.Kidney)
            {
                var temp = (KidneyFeatures)patientDiseaseFeature;
                diseaseFeatures = new KidneyFeatures()
                {
                    Albumin = temp.Albumin,
                    BloodglucoseRandom = temp.BloodglucoseRandom,
                    BloodUrea = temp.BloodUrea,
                    Haemoglobin = temp.Haemoglobin,
                    Hypertension = temp.Hypertension,
                    PackedCellVolume = temp.PackedCellVolume,
                    SerumCreatinine = temp.SerumCreatinine,
                    Sugar = temp.Sugar,
                    whiteBloodCellCount = temp.whiteBloodCellCount
                };
            }
            else if ((patient.Disease == Disease.Diabets))
            {
                var temp = (DiabetsFeatures)patientDiseaseFeature;
                diseaseFeatures = new DiabetsFeatures()
                {
                    BMI = temp.BMI,
                    Glucose = temp.Glucose,
                    Insulin = temp.Insulin,
                    SkinThickness = temp.SkinThickness,
                };

            }
            else if ((patient.Disease == Disease.BrainStroke))
            {
                var temp = (BrainStroke)patientDiseaseFeature;
                diseaseFeatures = new BrainStroke()
                {
                    BMI = temp.BMI,
                    AvgGlucose = temp.AvgGlucose,
                    HeartDisease = temp.HeartDisease,
                    Hypertension = temp.Hypertension,

                };

            }
            else
            {
                Cardiovascular temp = (Cardiovascular)patientDiseaseFeature;
                diseaseFeatures = new Cardiovascular()
                {
                    Ap_hi = temp.Ap_hi,
                    Ap_lo = temp.Ap_lo,
                    Cholesterol = temp.Cholesterol,
                    Height = temp.Height,
                    Weight = temp.Weight,
                };

            }

            //data sent to model

            var data = new DataSentToModelDto()
            {
                addReadingsDto = readingsDto,
                DiseaseFeatures = diseaseFeatures,
                Disease = patient.Disease,
                Age=patient.Age
            };
            ///send to the model        
            var url = "http://127.0.0.1:5000/classify-reading";
            var content = new StringContent(JsonSerializer.Serialize(data)/*readingsDto.ToString()*/, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var obj = JsonSerializer.Deserialize<ClassifiedData>(responseContent, options);

            var Reading = new Readings(readingsDto.BPM, readingsDto.AvgBPM, readingsDto.Temperature,obj.DeviceId,device.PatientId);
            Reading.PatientStatus = obj.classification;
            patient.PatientStatus = obj.classification;
            await patientRepository.Update(patient);
            await readingRepo.Add(Reading);

            ///send this reading to all doctors ,admins,this certain patient 
            await readingContext.Clients.All.SendAsync("UpdateReading", Reading);
            var All = await connectionIdsRepository.GetAll();
            List<UserWithConnectionId> UsersConnections= new List<UserWithConnectionId>();  
            foreach(var i in All)
            {
                var user = await userManager.FindByIdAsync(i.UserId);
                bool isPatient= await userManager.IsInRoleAsync(user, "Patient");
                if (!isPatient) UsersConnections.Add(i);
            }

            //notificationHub.Clients.AllExcept(UsersConnections.);
            

            return Ok();
        }


        [Authorize]
        [HttpGet("GetReccentPatientReading")]
        public async Task<ActionResult> GetReccentPatientReading(string? id)
        {
            var isPatient = User.IsInRole("Patient");

            string PatientId;
            if (isPatient)
            {
                var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                PatientId = user.Id;
            }
            else
            {
                if (string.IsNullOrEmpty(id)) return BadRequest(new ApiErrorResponse(400));
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return BadRequest(new ApiErrorResponse(400));
                PatientId = id;
            }
            
            var AllReadings=await readingRepo.GetAllPatientReadings(PatientId);
            var recentReading = AllReadings.FirstOrDefault();
            return Ok( new RecentReadingWithReadingCount(AllReadings.Count, recentReading));
        }
    }
}
