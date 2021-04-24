using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class WorkerSchedule
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Schedule Schedule { get; set; }
        public virtual Worker Worker { get; set; }
    }
}
