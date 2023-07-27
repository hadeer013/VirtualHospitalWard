using System;
using System.ComponentModel.DataAnnotations;
using VHM.DAL.Entities;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHM_APi_.Helper.Password_generator;

namespace VHM_APi_.EntityInputs
{
    public class PatientInput
    {
        [Required]
        public string UserName { get; set; }
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }= PasswordGeneration.Generator();
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public Disease Disease { get; set; }
        public string Address { get; set; }
        
    }
}
