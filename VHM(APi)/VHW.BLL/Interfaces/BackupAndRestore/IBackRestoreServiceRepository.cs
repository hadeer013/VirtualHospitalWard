using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;

namespace VHW.BLL.Interfaces.BackupAndRestore
{
    public interface IBackRestoreServiceRepository: IBackupAndResRepository
    {
        Task Restore(BackupFile backupTable);
        Task Backup(string Id);
    }
}

