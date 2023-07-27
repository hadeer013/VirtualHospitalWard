using Microsoft.AspNetCore.Http;
using System;
using VHM_APi_.EntityInputs;

namespace VHM_APi_.Dtos.Doctor
{
    public class DoctorDto:ApplicationUserDto
    {
        public string Department { get; set; }
    }
}
