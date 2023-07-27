using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data;
using VHM.DAL.Data.HangFireContext;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHM.DAL.Entities;
using VHW.BLL.Interfaces.BackupAndRestore;
using VHW.BLL.Services.BackupService;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories.HangFireRepository
{
    public class BackupAndResRepository : IBackupAndResRepository
    {
        private readonly HangFireDbContext fireDbContext;

        public BackupAndResRepository(HangFireDbContext fireDbContext)
        {
            this.fireDbContext = fireDbContext;
        }



        public async Task<int> Add(BackupFile type)
        {
            fireDbContext.Add(type);
            return await fireDbContext.SaveChangesAsync();
        }



        public async Task<IReadOnlyList<BackupFile>> GetAllWithSpec(ISpecification<BackupFile> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<BackupFile> ApplySpecification(ISpecification<BackupFile> spec)
        {
            return SpecificationEvaluator<BackupFile>.BuildQuery(fireDbContext.BackupFiles, spec);
        }

        public async Task<int> GetCountAsync(ISpecification<BackupFile> spec)
        => await ApplySpecification(spec).CountAsync();
        //**********************************************
        public async Task<BackupFile> GetWithId(string id)
        {
            return await fireDbContext.BackupFiles.FindAsync(id);
        }

        public async Task<int> Update(BackupFile type)
        {
            fireDbContext.Update(type);
            return await fireDbContext.SaveChangesAsync();
        }
    }
}
