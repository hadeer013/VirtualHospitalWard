using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Entities;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.ConsultaTask;
using VHM_APi.Helper;
using VHM_APi_.Dtos;
using VHM_APi_.Dtos.ConsultationTask;
using VHM_APi_.Dtos.VideoCall;
using VHM_APi_.Errors;
using VHM_APi_.Helper;
using VHM_APi_.Helper.Meeting_Status;
using VHM_APi_.Hubs;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;
using VHW.BLL.Specification.ConsultationTaskSpec;

namespace VHM_APi_.Controllers
{
    public class ConsultaionTaskController : BaseApiController
    {
        private readonly IConsultationTask TaskRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly IUserWithConnectionIdsRepository connectionIdsRepository;
        private readonly IGenericRepository<Notification> notificationRepo;
        private readonly int apiKey = 123;
        private readonly string apiSecret = "";

        public ConsultaionTaskController(IConsultationTask TaskRepo, UserManager<ApplicationUser> userManager
            , IHubContext<NotificationHub> NotificationHub, IUserWithConnectionIdsRepository connectionIdsRepository, IGenericRepository<Notification> notificationRepo)
        {
            this.TaskRepo = TaskRepo;
            this.userManager = userManager;
            notificationHub = NotificationHub;
            this.connectionIdsRepository = connectionIdsRepository;
            this.notificationRepo = notificationRepo;
        }
         

        //[Authorize(Roles = "Doctor,Admin,Patient")]
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<AllTasksObject>>> GetAllConsultationTask(Month month)
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await userManager.FindByEmailAsync(email);
        //    var result = await TaskRepo.GetConsultaionTasksForWeek(user.Id);
        //    var ans = new List<AllTasksObject>();

        //    for (int i = 0; i < result.Count; i++)
        //    {
        //        var temp = new AllTasksObject();
        //        temp.TaskDate = result[i].TaskDate;

        //        foreach (var item in result[i].Tasks)
        //        {
        //            var CDto = new ConsultationDto()
        //            {
        //                Id = item.Id,
        //                TaskType = item.TaskType,
        //                date = item.TaskDate,
        //                To = item.To
        //            };
        //            temp.Consultations.Add(CDto);
        //        }
        //        temp.Count = result[i].Tasks.Count;
        //        ans.Add(temp);
        //    }

        //    return Ok(ans);
        //}
        //

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AllTasksObject>>> GetAllConsultationTask()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var result = await TaskRepo.GetConsultaionTasksForWeek(user.Id);
            var ans = new List<AllTasksObject>();

            for (int i = 0; i < result.Count; i++)
            {
                var temp = new AllTasksObject();
                temp.TaskDate = result[i].TaskDate;

                foreach (var item in result[i].Tasks)
                {
                    var CDto = new ConsultationDto()
                    {
                        Id = item.Id,
                        TaskType = item.TaskType,
                        date = item.TaskDate,
                        To = item.To
                    };
                    temp.Consultations.Add(CDto);
                }
                temp.Count = result[i].Tasks.Count;
                ans.Add(temp);
            }

            return Ok(ans);
        }



        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("GetTask/{id}")]
        public async Task<ActionResult<ConsultationDetailsDto>> GetTaskById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var task = await TaskRepo.GetWithId(id);
            if (task != null)
            {
                var IsDoctor = User.IsInRole("Doctor");
                if (IsDoctor)
                {
                    if (!(user.Id == task.UserRecieverId || user.Id == task.UserInitializerId))
                        return BadRequest(new ApiErrorResponse(400));
                }
                var Reciever = await userManager.FindByIdAsync(task.UserRecieverId);
                var Consultation_dto = new ConsultationDetailsDto()
                {
                    Id = task.Id,
                    To = task.To,
                    RecieverName = Reciever.UserName,
                    TaskType = task.TaskType,
                    TaskDate = task.TaskDate
                };
                return Ok(Consultation_dto);
            }
            return NotFound(new ApiErrorResponse(404));
        }


        [Authorize(Roles = "Doctor,Admin,Patient")]
        [HttpPost]
        public async Task<ActionResult<ConsultaionTask>> AddTask(AddConsultationTaskDto task)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
             var RecieverUser = await userManager.FindByNameAsync(task.userRecieverName);

            if (RecieverUser != null)
            {

                if (!(task.TaskDate > DateTime.Now)) return BadRequest(new ApiErrorResponse(400,"The meeting date has already been passed"));
                var jobId = BackgroundJob.Schedule(() => CreateAudioOrVideoCall(), task.TaskDate - DateTime.Now);
                var All = await connectionIdsRepository.GetAll();
                var userWithConnectionId = All.Where(u => u.UserId == RecieverUser.Id).FirstOrDefault();
                var notif = new Notification() { Content = $"{user.UserName} wants to have a meeting with you at {task.TaskDate}", date = DateTime.Now, SenderId = user.Id, RecieverId = RecieverUser.Id };
                await notificationRepo.Add(notif);
                if (userWithConnectionId != null)
                {
                    await notificationHub.Clients.Client(userWithConnectionId.ConnectionId)
                        .SendAsync("ReceiveNotification", $"{user.UserName} wants to have a meeting with you at {task.TaskDate}", DateTime.Now, user.Id);
                }
                var ConsultationTask = new ConsultaionTask()
                {
                    TaskType = task.TaskType,
                    TaskDate = task.TaskDate,
                    To = task.To,
                    JobId = jobId,
                    UserInitializerId = user.Id,
                    UserRecieverId = RecieverUser.Id
                };
                var result = await TaskRepo.Add(ConsultationTask);
                var consultaion = new ConsultationDetailsDto()
                {
                    Id = result.Id,
                    TaskDate = task.TaskDate,
                    TaskType = task.TaskType,
                    RecieverName = RecieverUser.UserName,
                    To = task.To,
                    Status = MeetingStatus.Pending,
                };
                return Ok(consultaion);
            }
            return NotFound(new ApiErrorResponse(404));
        }


        [Authorize(Roles = "Doctor,Admin,Patient")]
        [HttpPost("ChangeMeetingStatus")]
        public async Task<ActionResult<ConsultationDetailsDto>> ChangeMeetingTaskStatus(MeetingInputParameters meetingParameters)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var result = await TaskRepo.GetWithId(meetingParameters.TaskId);
            if (result != null)
            {
                if (meetingParameters.TaskDate != result.TaskDate || meetingParameters.To != result.To)
                {
                    if (!(meetingParameters.TaskDate > DateTime.Now)) return BadRequest(new ApiErrorResponse(400));
                    //get job with the spcified Jobid
                    var jobData = BackgroundJob.Delete(result.JobId);
                    if (jobData)
                    {
                        var NewjobId = BackgroundJob.Schedule(() => CreateAudioOrVideoCall(), meetingParameters.TaskDate - DateTime.Now);
                        result.JobId = NewjobId;
                    }
                    else return BadRequest(new ApiErrorResponse(400));

                    result.To = meetingParameters.To;
                    result.TaskDate = meetingParameters.TaskDate;
                }
                result.Status = meetingParameters.Status;

                await TaskRepo.Update(result);
                return Ok(new ConsultationDetailsDto()
                {
                    Id = meetingParameters.TaskId,
                    TaskType = result.TaskType,
                    TaskDate = result.TaskDate,
                    RecieverName = user.UserName,
                    To = result.To,
                    Status = result.Status
                });
            }
            return BadRequest(new ApiErrorResponse(400));
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("CreateAudioOrVideoCall")]
        public async Task<ActionResult<VideoCallDTO>> CreateAudioOrVideoCall()
        {
            var opentok = new OpenTok(apiKey, apiSecret);
            var session = opentok.CreateSession();
            var sessionId = session.Id;
            var publisherToken = session.GenerateToken();
            string subscriberToken = opentok.GenerateToken(sessionId, Role.SUBSCRIBER);
            var VideoCallDTO = new VideoCallDTO
            {
                SessionId = session.Id,
                PublisherToken = publisherToken,
                SubscriberToken = subscriberToken,
                ApiKey = apiKey
            };
            return Ok(VideoCallDTO);
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("SubscribeToCall")]
        public ActionResult SubscribeToVideoCall(string sessionId)
        {
            var opentok = new OpenTok(apiKey, apiSecret);

            var subscriberToken = opentok.GenerateToken(sessionId, Role.SUBSCRIBER);
            var obj = new SubscribeToVideoCallDto() { SessionId = sessionId, SubscriberToken = subscriberToken, ApiKey = apiKey };
            return Ok(obj);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllAppointments")]
        public async Task<ActionResult<Pagination<appointmetDto>>> GetAllAppointments([FromQuery] BaseFilterationParams baseFilterationParams)
        {
            var spec = new ConsultationTaskSpecification(baseFilterationParams);
            var tasks = await TaskRepo.GetAllWithSpec(spec);
            var counSpec = new ConsultationTaskwithFilterForCountSpec(baseFilterationParams);
            IEnumerable<appointmetDto> res = new List<appointmetDto>();
            foreach(var task in tasks)
            {
                var temp = new appointmetDto()
                {
                    TaskId = task.Id,
                    date = task.TaskDate,
                    InitiatorId = task.UserInitializerId,
                    InitiatorName = task.UserInitializer.UserName,
                    RecieverId = task.UserRecieverId,
                    ReceiverName = task.UserReciever.UserName,
                    Status = task.Status
                };
               res= res.Append(temp);
            }

            var result = new Pagination<appointmetDto>(baseFilterationParams.PageIndex
                , baseFilterationParams.PageSize, await TaskRepo.GetCountAsync(counSpec), res.ToList());
            return Ok(result);
        }
    }
}
