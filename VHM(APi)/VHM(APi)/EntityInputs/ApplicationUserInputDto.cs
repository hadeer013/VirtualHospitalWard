using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VHM_APi_.EntityInputs
{
    public class ApplicationUserInputDto
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public IFormFile Image { get; set; }

    }
}
