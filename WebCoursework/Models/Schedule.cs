using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("День")]
        public int DayId { get; set; }
        [DisplayName("Відрізок часу")]
        public int TimeSegmentId { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("День")]
        public virtual Day Day { get; set; }
        public virtual TimeSegment TimeSegment { get; set; }
        public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; }
    }
}
