using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHW.BLL.Services.BackupService;

namespace VHW.BLL.Specification.BackupSpec
{
    public class BackupWithFiltersForBackupSpecification : BaseSpecification<BackupFile>
    {
        public BackupWithFiltersForBackupSpecification(BaseFilterationParams BackupParams) :
            base(b => (string.IsNullOrEmpty(BackupParams.Search) || b.AdminName.ToLower().Contains(BackupParams.Search))&&b.AlreadyEnqueued)
        {
        }
    }
}
