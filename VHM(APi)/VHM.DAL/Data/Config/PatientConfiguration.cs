using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.PatientEntities;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM.DAL.Data.Config
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(o => o.Disease)
            .HasConversion(oStatus => oStatus.ToString(),
             oStatus => (Disease)Enum.Parse(typeof(Disease), oStatus));

            builder.Property(o => o.PatientStatus)
            .HasConversion(oStatus => oStatus.ToString(),
             oStatus => (PatientStatus)Enum.Parse(typeof(PatientStatus), oStatus));

            builder.HasOne(p => p.DiseaseFeatures).WithOne(d => d.Patient);

        }
    }
}
