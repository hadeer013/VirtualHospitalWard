using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Data.HangFireContext.HangFContextEntities
{
    public class BackupFile
    {
        public BackupFile()
        {
        }

        public BackupFile(string id, string adminId, string adminName, string filePath, string fileName,DateTime date)
        {
            Id = id;
            AdminId = adminId;
            AdminName = adminName;
            FilePath = filePath;
            FileName = fileName;
            Date = date;
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } 
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public bool AlreadyEnqueued { get; set; } = false;
    }
}
