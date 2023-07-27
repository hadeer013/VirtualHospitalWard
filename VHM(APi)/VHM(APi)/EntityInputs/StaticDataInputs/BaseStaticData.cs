using System.ComponentModel.DataAnnotations;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM_APi_.EntityInputs.StaticDataInputs
{
    public class BaseStaticData
    {
        [Required]
        public string PatientId { get; set; }
    }
}
