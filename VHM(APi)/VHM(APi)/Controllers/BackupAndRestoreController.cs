using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHW.BLL.Interfaces.BackupAndRestore;
using VHM_APi_.Errors;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Identity;
using VHM.DAL.Entities;
using System.Security.Claims;
using VHM_APi_.Dtos;
using AutoMapper;
using VHW.BLL.Services.BackupService;
using VHW.BLL.Specification;
using VHW.BLL.Specification.BackupSpec;
using Talabat.BLL.Specification.ProductSpecification;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi.Helper;

namespace VHM_APi_.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BackupAndRestoreController : BaseApiController
    {
        private readonly IBackRestoreServiceRepository backRestoreServiceRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public BackupAndRestoreController(IBackRestoreServiceRepository backRestoreServiceRepository,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.backRestoreServiceRepository = backRestoreServiceRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BackupFile>>> GetAllBackups([FromQuery] BaseFilterationParams filterationParams)
        {
            var spec = new BackupSpecification(filterationParams);
            var backs = await backRestoreServiceRepository.GetAllWithSpec(spec);
            var count = new BackupWithFiltersForBackupSpecification(filterationParams);
            var page = new Pagination<BackupFile>(filterationParams.PageIndex, filterationParams.PageSize, await backRestoreServiceRepository.GetCountAsync(count), backs);
            return Ok(page);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("backup")]
        public async Task<ActionResult> Backup(DateTime dateTime)
        {
            try
            {
                var delay = dateTime - DateTime.Now;
                string Id = Guid.NewGuid().ToString();
                var admin = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                BackgroundJob.Schedule(() => backRestoreServiceRepository.Backup(Id), delay);
                string DateFormat = dateTime.ToString("yyyy-M-dd hh-mm");
                string fileName = "dataBackup" + DateFormat;
                string FilePath = "D:\\backups\\" + fileName + ".bak";
                BackupFile bak = new BackupFile(Id, admin.Id, admin.UserName, FilePath, fileName, dateTime);
                await backRestoreServiceRepository.Add(bak);
                var result = new BackupDto()
                {

                    FileName = fileName,
                    Datetime = dateTime,
                    AdminName = admin.UserName
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("restore")]
        public async Task<ActionResult<BackupFile>> Restore(string backupFileId)
        {
            var backupFile = await backRestoreServiceRepository.GetWithId(backupFileId);
            if (backupFile == null) return BadRequest(new ApiErrorResponse(400));
            try
            {
                await backRestoreServiceRepository.Restore(backupFile);
                return Ok(backupFile);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiErrorResponse(400, e.Message));
            }

        }
    }
}
