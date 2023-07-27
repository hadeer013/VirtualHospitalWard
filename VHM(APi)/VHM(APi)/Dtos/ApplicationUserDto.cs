using Microsoft.AspNetCore.Http;

namespace VHM_APi_.Dtos
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string ImageUrl { get; set; }
    }
}
