
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHM.DAL.Entities.ChatEntities;

namespace ChatWithAuth.DAL.Data.Config
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(m => m.ReceiverUser).WithMany();
            builder.HasOne(m => m.SenderUser).WithMany();
            
        }
    }
}
