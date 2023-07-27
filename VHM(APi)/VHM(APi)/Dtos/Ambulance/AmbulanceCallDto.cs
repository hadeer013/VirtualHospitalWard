using System.ComponentModel.DataAnnotations.Schema;
using System;
using VHM.DAL.Entities.Ambulance;
using VHM.DAL.Entities.PatientEntities;

namespace VHM_APi_.Dtos.Ambulance
{
    public class AmbulanceCallDto
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string PatientUserName { get; set; }
        public PatientLocation Location { get; set; }
        public DateTime Date { get; set; }
        public RequestStatus Status { get; set; }
    }
}
