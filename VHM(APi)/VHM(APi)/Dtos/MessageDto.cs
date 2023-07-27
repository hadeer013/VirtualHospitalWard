using System;

namespace VHM_APi_.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Contetnt { get; set; }
        public DateTime date { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public bool IsSent { get; set; } = false;
    }
}
