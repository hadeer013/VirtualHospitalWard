using System.ComponentModel.DataAnnotations;

namespace VHM_APi_.Dtos.Devic
{
    public class PatientDeviceAssignmentDto
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public string PatientId { get; set; }
    }
}
