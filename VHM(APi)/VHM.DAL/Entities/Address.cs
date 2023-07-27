using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM.DAL.Entities
{
    public class Address:BaseEntity
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Appartment { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }

    }
}
