using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHW.BLL.Specification;

namespace VHW.BLL.Interfaces
{
    public interface IUserRepository<T> where T : ApplicationUser
    {
        Task<T> GetWithId(string id);
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAll();
        Task<int> Update(T type);
        Task<int> Delete(T type);

        Task<T> GetByIdWithSpec(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task<int> GetCount();
        
    }
}
