using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.DoctorEntities;
using VHW.BLL.Specification.PatientSpec;

namespace VHW.BLL.Specification.DoctorSpec
{
    public class DoctorSpecification : BaseSpecification<Doctor>
    {
        public DoctorSpecification(DoctorParams DoctorParams)
          : base(p => (string.IsNullOrEmpty(DoctorParams.Search) || (p.UserName.ToLower().Contains(DoctorParams.Search)))
                &&string.IsNullOrEmpty(DoctorParams.Department )|| p.Department.ToLower().Contains(DoctorParams.Department))
        {
            AddOrderBy(p => p.UserName);
            ApplyPagination((DoctorParams.PageSize * (DoctorParams.PageIndex - 1)), DoctorParams.PageSize);

        }
        public DoctorSpecification(string id) : base((Doctor) => Doctor.Id == id)
        {
            
            
        }
    }
}
