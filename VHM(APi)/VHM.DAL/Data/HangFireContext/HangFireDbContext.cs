using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;

namespace VHM.DAL.Data.HangFireContext
{
    public class HangFireDbContext : DbContext
    {
        public HangFireDbContext(DbContextOptions<HangFireDbContext> options) : base(options)
        {
        }
        public DbSet<BackupFile> BackupFiles { get; set; }
    }
}
