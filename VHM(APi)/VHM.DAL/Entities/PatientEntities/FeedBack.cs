using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities.PatientEntities
{
    public class FeedBack: BaseEntity
    {
       public string content { get; set; }
        private string rateValue;
       public DateTime CreationDate { get; set; }= DateTime.Now;
       public string PatientId { get; set; }

       [ForeignKey(nameof(PatientId))]
       public virtual Patient Patient { get; set; }

        public string RateValue
        {
            get { return rateValue; }
            set { rateValue = string.IsNullOrEmpty(value)?"0":value; }
        }

    }
}
