using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHW.BLL.Specification;

namespace VHW.BLL.Interfaces
{
    public interface IGenericRepository<T> where T:BaseEntity
    {
        Task<T> GetWithId(int? id);  
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetByIdWithSpec(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task<int> GetCount();
        Task<T> Add(T type);
        Task<int> Update(T type);
    }
}
