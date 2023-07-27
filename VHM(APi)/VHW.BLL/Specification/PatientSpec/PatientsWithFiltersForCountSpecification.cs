using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;
using VHW.BLL.Specification.PatientSpec;

namespace Talabat.BLL.Specification.ProductSpecification
{
    public class PatientsWithFiltersForCountSpecification : BaseSpecification<Patient>
    {
        public PatientsWithFiltersForCountSpecification(PatientParams productParams) 
            : base(p => (string.IsNullOrEmpty(productParams.Search) || (p.UserName.ToLower().Contains(productParams.Search)))
                  && (productParams.SearchByDisease.HasValue && p.Disease == productParams.SearchByDisease))
        {
        }
    }
}
