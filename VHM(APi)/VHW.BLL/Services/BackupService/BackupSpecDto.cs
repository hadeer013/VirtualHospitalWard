using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHW.BLL.Services.BackupService
{
    public class BackupSpecDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime Datetime { get; set; }
        public string AdminId { get; set; }
        public string AdminName { get; set; }
    }
}
