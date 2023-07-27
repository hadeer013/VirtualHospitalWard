using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.Feeling;
using VHM_APi_.Errors;

namespace VHM_APi_.Controllers
{
    public class FeelingController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public FeelingController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        //[Authorize(Roles = "Patient")]
        //[HttpPost]
        //public async Task<ActionResult> GetPatientFeeling(FeelingStatus feelingStatus)
        //{
        //    var mail = User.FindFirstValue(ClaimTypes.Email);
        //    var user = userManager.FindByEmailAsync(mail);
             

        //}
    }
}
