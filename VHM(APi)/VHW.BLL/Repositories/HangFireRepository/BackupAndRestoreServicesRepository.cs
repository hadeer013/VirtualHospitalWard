using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Data;
using VHM.DAL.Data.HangFireContext;
using VHM.DAL.Data.HangFireContext.HangFContextEntities;
using VHW.BLL.Interfaces.BackupAndRestore;

namespace VHW.BLL.Repositories.HangFireRepository
{
    public class BackupAndRestoreServicesRepository : BackupAndResRepository, IBackRestoreServiceRepository
    {
        private readonly IConfiguration configuration;
        private readonly HangFireDbContext fireDbContext;

        public BackupAndRestoreServicesRepository(HangFireDbContext fireDbContext, IConfiguration configuration) : base(fireDbContext)
        {
            this.fireDbContext = fireDbContext;
            this.configuration = configuration;
        }

        public async Task Backup(string Id)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            con.ConnectionString = configuration.GetConnectionString("DefaultConnection");

            var databaseName = "VHWAPi";
            con.Open();
            DateTime Date = DateTime.Now;
            string DateFormat = Date.ToString("yyyy-M-dd hh-mm");
            string fileName = "dataBackup" + DateFormat;
            sqlcmd = new SqlCommand($"backup database {databaseName} to disk = 'D:\\backups\\{fileName}.bak'", con);
            await sqlcmd.ExecuteNonQueryAsync();
            await con.CloseAsync();
            var BackupFile = await fireDbContext.BackupFiles.FindAsync(Id);
            BackupFile.AlreadyEnqueued = true;
            await Update(BackupFile);
        }

        public async Task Restore(BackupFile backupTable)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();


            con.ConnectionString = configuration.GetConnectionString("DefaultConnection");




            try
            {
                var databaseName = "VHWAPi";
                con.Open();

                sqlcmd = new SqlCommand($"alter database {databaseName} set offline with rollback immediate", con);
                await sqlcmd.ExecuteNonQueryAsync();

                sqlcmd = new SqlCommand($"restore database {databaseName} from disk = '{backupTable.FilePath}'", con);
                await sqlcmd.ExecuteNonQueryAsync();

                sqlcmd = new SqlCommand($"alter database {databaseName} set online", con);
                await sqlcmd.ExecuteNonQueryAsync();

                await con.CloseAsync();


            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

