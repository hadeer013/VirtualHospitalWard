using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;
using VHW.BLL.Specification.PatientSpec;

namespace Talabat.BLL.Specification
{
    public class PatientWithReportSpecification : BaseSpecification<Patient>
    {
        public PatientWithReportSpecification(PatientParams patientParams)
          : base(p => (string.IsNullOrEmpty(patientParams.Search) || (p.UserName.ToLower().Contains(patientParams.Search)))
                  &&  (patientParams.SearchByDisease.HasValue && p.Disease == patientParams.SearchByDisease))       
        {
            AddOrderBy(p => p.UserName);
            ApplyPagination((patientParams.PageSize * (patientParams.PageIndex - 1)), patientParams.PageSize);

        }
        public PatientWithReportSpecification(string id) : base((patient) => patient.Id == id)
        {
            //AddOrderBy(p => p.UserName);
            //AddInclude((patient) => patient.Address);
            //AddInclude((patient) => patient.Readings);
            //AddInclude((patient) => patient.FeedBacks);
            //AddInclude((patient) => patient.Prescriptions);
            //AddInclude((patient) => patient.Reports);
        }
    }
}
