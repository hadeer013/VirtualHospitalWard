using System;

namespace VHM_APi_.Dtos
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string DoctorName { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
