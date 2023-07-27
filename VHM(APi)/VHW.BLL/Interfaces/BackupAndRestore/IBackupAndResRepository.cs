using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHW.BLL.Services.BackupService;
using VHW.BLL.Specification;

namespace VHW.BLL.Interfaces.BackupAndRestore
{
    public interface IBackupAndResRepository
    {
        Task<int> Add(BackupFile type);
        Task<BackupFile> GetWithId(string id);
        Task<int> GetCountAsync(ISpecification<BackupFile> spec);
        Task<IReadOnlyList<BackupFile>> GetAllWithSpec(ISpecification<BackupFile> spec);
        Task<int> Update(BackupFile type);
    }

}
