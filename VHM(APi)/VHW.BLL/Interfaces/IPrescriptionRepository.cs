﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;

namespace VHW.BLL.Interfaces
{
    public interface IPrescriptionRepository:IGenericRepository<Prescription>
    {
        Task<IReadOnlyList<Prescription>> GetPrescriptionsByPatientId(string Patientid);
    }
}
