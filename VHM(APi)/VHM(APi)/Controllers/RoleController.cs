using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VHM_APi_.Controllers
{
    
    public class RoleController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<IdentityRole>> AddRole(IdentityRole identityRole)
        {
            var result= await roleManager.CreateAsync(identityRole);
            if(result.Succeeded)
                return identityRole;
            return BadRequest();
        }
    }
}
