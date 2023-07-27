using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.DoctorEntities;

namespace VHM.DAL.Data.Config
{
    public class DoctorWithSavedConfiguration : IEntityTypeConfiguration<UserWithSavedPatient>
    {
        public void Configure(EntityTypeBuilder<UserWithSavedPatient> builder)
        {
            builder.HasKey(k => new { k.PatientId, k.UserId });
        }
    }
}
