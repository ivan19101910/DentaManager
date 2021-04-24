using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class Schedule
    {
        public Schedule()
        {
            WorkerSchedules = new HashSet<WorkerSchedule>();
        }

        public int ScheduleId { get; set; }
        public int DayId { get; set; }
        public int TimeSegmentId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Day Day { get; set; }
        public virtual TimeSegment TimeSegment { get; set; }
        public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; }
    }
}
