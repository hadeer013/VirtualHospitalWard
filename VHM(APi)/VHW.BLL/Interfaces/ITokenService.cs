using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;

namespace VHW.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser appUser, UserManager<ApplicationUser> userManager);
    }
}
