using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Data;
using VHM.DAL.Entities;
using VHM.DAL.Entities.PatientEntities;

using VHW.BLL.Interfaces;
using VHW.BLL.Specification;

namespace VHW.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly HospitalContext hospitalContext;

        public GenericRepository(HospitalContext hospitalContext)
        {
            this.hospitalContext = hospitalContext;

        }
        public async Task<T> GetWithId(int? id)
        {
            return await hospitalContext.Set<T>().FindAsync(id);
        }
        public async Task<T> Add(T type)
        {
            var result= await hospitalContext.AddAsync(type);
             hospitalContext.SaveChanges();
            return result.Entity;
        }
        public async Task<IReadOnlyList<T>> GetAll()
        {
          return await hospitalContext.Set<T>().ToListAsync();
        }







        ///*******************************

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        //**********************************
        //query builder
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.BuildQuery(hospitalContext.Set<T>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).CountAsync();

        public async Task<int> GetCount()
        {
            var list= await (hospitalContext.Set<T>().ToListAsync());
            return list.Count;
        }

        public async Task<int> Update(T type)
        {
            hospitalContext.Set<T>().Update(type);
            return await hospitalContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
    }
}
