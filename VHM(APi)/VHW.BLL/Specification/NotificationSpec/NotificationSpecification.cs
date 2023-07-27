using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specification;
using VHM.DAL.Entities.ChatEntities;
using VHW.BLL.Specification.FeedBackSpec;

namespace VHW.BLL.Specification.NotificationSpec
{
    public class NotificationSpecification:BaseSpecification<Notification>
    {
        public NotificationSpecification(string UserId)
          : base(p => p.RecieverId== UserId /*&& !p.IsRead*/)
        {
            AddInclude(f => f.Sender);
            AddOrderByDesc(p => p.date);
            
        } 
    }
}
