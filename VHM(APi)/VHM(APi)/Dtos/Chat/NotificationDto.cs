using System.ComponentModel.DataAnnotations.Schema;
using System;
using VHM.DAL.Entities;

namespace VHM_APi_.Dtos.Chat
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime date { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string RecieverId { get; set; }
        public bool IsRead { get; set; }
    }
}
