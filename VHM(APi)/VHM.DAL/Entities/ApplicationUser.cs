using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VHM.DAL.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string ImageUrl { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;

    }
}
