using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data;
using VHM.DAL.Entities;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories
{
    public class DoctorRepository: UserRepository<Doctor>,IDoctorRepository
    {
        private readonly HospitalContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public DoctorRepository(HospitalContext context,UserManager<ApplicationUser> userManager):base(context, userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IReadOnlyList<Doctor>> GetOnlineDoctor()
        {
            var DoctorRole = await context.Roles.Where(r => r.Name == "Doctor").FirstOrDefaultAsync();
            var result =await context.UserWithConnectionIds.Join(context.UserRoles,
                u => u.UserId, r => r.UserId, (u, r) => new { u.UserId, r.RoleId }).Where(r => r.RoleId == DoctorRole.Id).Select(a=>a.UserId).ToListAsync();

            IReadOnlyList<Doctor> doctors = new List<Doctor>();
            foreach (var doctor in result)
            {
                var user =(Doctor) await userManager.FindByIdAsync(doctor);
                doctors.Append(user);
            }
            return doctors;
        }
    }
}
