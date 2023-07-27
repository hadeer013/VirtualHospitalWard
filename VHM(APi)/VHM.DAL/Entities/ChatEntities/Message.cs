﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.ChatEntities
{
    public class Message : BaseEntity
    {
        public string Contetnt { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

        public string ReceiverUserId { get; set; }
        [ForeignKey(nameof(ReceiverUserId))]
        public ApplicationUser ReceiverUser { get; set; }


        public string SenderUserId { get; set; }
        [ForeignKey(nameof(SenderUserId))]
        public ApplicationUser SenderUser { get; set; }

        public bool IsRead { get; set; } = false;

    }
}
