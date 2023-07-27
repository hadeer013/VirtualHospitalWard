using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities;
using VHM.DAL.Entities.Ambulance;

namespace VHM.DAL.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(o => o.Gender)
           .HasConversion(oStatus => oStatus.ToString(),
            oStatus => (Gender)Enum.Parse(typeof(Gender), oStatus));
        }
    }
}
