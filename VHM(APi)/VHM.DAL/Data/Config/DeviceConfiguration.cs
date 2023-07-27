using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DevicesEntities;
using VHM.DAL.Entities.PatientEntities;

namespace VHM.DAL.Data.Config
{

    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasOne(D => D.Patient)
                   .WithOne()
                   .IsRequired(false);
        }
    }
}
