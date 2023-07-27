using System;

namespace VHM_APi_.Dtos
{
    public class SupportMessageDto
    {
        public int Id { get; set; } 
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string PatientName { get; set; }
        public string PatientId { get; set; }
    }
}
