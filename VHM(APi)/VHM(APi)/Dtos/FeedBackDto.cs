using System;

namespace VHM_APi_.Dtos
{
    public class FeedBackDto
    {
        public int Id { get; set; } 
        public string PatientName {get; set; }
        public string RateValue { get; set; }
        public string content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
