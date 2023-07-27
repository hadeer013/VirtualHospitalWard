using VHM_APi_.Dtos;

namespace VHM_APi_.EntityInputs.DoctorInputs
{
    public class UpdateDoctorInputDto:ApplicationUserInputDto
    {
        public string Department { get; set; }
    }
}
