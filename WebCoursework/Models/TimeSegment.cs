using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class TimeSegment
    {
        public TimeSegment()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int TimeSegmentId { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
