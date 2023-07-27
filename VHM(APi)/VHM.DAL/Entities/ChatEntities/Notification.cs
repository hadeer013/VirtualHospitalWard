using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ChatEntities
{
    public class Notification:BaseEntity
    {
        public string Content { get; set; }
        public DateTime date { get; set; }=DateTime.Now;
        public string SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; }
        public string RecieverId { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
