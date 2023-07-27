using System.Collections.Generic;
using VHM.DAL.Entities.ReadingEntities;

namespace VHM_APi_.Helper
{
    public class RecentReadingWithReadingCount
    {
        public RecentReadingWithReadingCount(int numberOfReadingPerformed, Readings recentReading)
        {
            NumberOfReadingPerformed = numberOfReadingPerformed;
            RecentReading = recentReading;
        }

        public int NumberOfReadingPerformed { get; set; } = 0;
        public Readings? RecentReading { get; set; }
    }
}
