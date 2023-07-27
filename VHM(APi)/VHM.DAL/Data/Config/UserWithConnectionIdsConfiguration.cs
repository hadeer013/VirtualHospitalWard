using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ChatEntities;
using VHM.DAL.Entities.DoctorEntities;

namespace VHM.DAL.Data.Config
{
    public class UserWithConnectionIdsConfiguration:IEntityTypeConfiguration<UserWithConnectionId>
    {
        public void Configure(EntityTypeBuilder<UserWithConnectionId> builder)
        {
            builder.HasKey(k => new { k.ConnectionId });
        }
    }
}
