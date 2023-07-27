using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ConsultaTask;

namespace VHM.DAL.Data.Config
{
    public class ConsultationTaskConfiguration : IEntityTypeConfiguration<ConsultaionTask>
    {
        public void Configure(EntityTypeBuilder<ConsultaionTask> builder)
        {
            builder.HasOne(t => t.UserInitializer).WithMany();
            builder.HasOne(t => t.UserReciever).WithMany();
            builder.Property(o => o.Status)
                .HasConversion(oStatus => oStatus.ToString(),
                oStatus => (MeetingStatus)Enum.Parse(typeof(MeetingStatus), oStatus));
        }
    }
}
