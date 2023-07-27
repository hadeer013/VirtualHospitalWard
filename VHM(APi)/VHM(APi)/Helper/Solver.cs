using AutoMapper;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;
using VHM.DAL.Entities.PatientEntities;
using VHM_APi_.EntityInputs;
using VHW.BLL.Interfaces;

namespace VHM_APi_.Helper
{
    public class Solver : IValueResolver<PrescriptionInput, Prescription,Task< Patient>>
    {
        private readonly IPatientRepository patientRepository;

        public Solver(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<Patient> Resolve(PrescriptionInput source, Prescription destination, Task<Patient> destMember, ResolutionContext context)
        {
            var patient = patientRepository.GetWithId(source.PatientId);
            return await patient;
        }

    }
}
