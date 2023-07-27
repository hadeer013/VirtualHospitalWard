using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.Ambulance;
using VHM.DAL.Entities.ConsultaTask;

namespace VHM.DAL.Data.Config
{
    public class AmbulanceCallConfiguration:IEntityTypeConfiguration<AmbulanceCall>
    {
        public void Configure(EntityTypeBuilder<AmbulanceCall> builder)
        {
            builder.OwnsOne(o => o.Location, Location => Location.WithOwner());
            builder.Property(p => p.Status)
                .HasConversion(s => s.ToString(),
                s => (RequestStatus)Enum.Parse(typeof(RequestStatus), s));
        }
    }
}
