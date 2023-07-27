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
using VHW.BLL.Interfaces;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories
{
    public class UserRepository<T> : IUserRepository<T> where T : ApplicationUser
    {
        private readonly HospitalContext hospitalContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(HospitalContext hospitalContext,UserManager<ApplicationUser> userManager)
        {
            this.hospitalContext = hospitalContext;
            this.userManager = userManager;
        }
        public async Task<T> GetWithId(string id)
        {
            return await hospitalContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await hospitalContext.Set<T>().ToListAsync();
        }



        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.BuildQuery(hospitalContext.Set<T>(), spec);
        }
        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }



       
        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(T type)
        {
            await userManager.UpdateAsync(type);
            return await hospitalContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T type)
        {
            await userManager.DeleteAsync(type);
            return await hospitalContext.SaveChangesAsync();
        }


    }
}
