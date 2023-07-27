using System;
using System.ComponentModel.DataAnnotations;
using VHM_APi_.Helper.Password_generator;

namespace VHM_APi_.EntityInputs
{
    public class AdminStaffAdditionInput
    {
        [Required]
        public string UserName { get; set; }
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; } = PasswordGeneration.Generator();
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
