using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHW.BLL.Services.BackupService;
using VHW.BLL.Specification.ReportSpec;

namespace VHW.BLL.Specification.BackupSpec
{
    public class BackupSpecification : BaseSpecification<BackupFile>
    {
        public BackupSpecification(BaseFilterationParams BackupParams) :
            base(b => (string.IsNullOrEmpty(BackupParams.Search) || b.AdminName.ToLower().Contains(BackupParams.Search))&&b.AlreadyEnqueued)
        {
            ApplyPagination(BackupParams.PageSize * (BackupParams.PageIndex - 1), BackupParams.PageSize);
        }
    }
}
