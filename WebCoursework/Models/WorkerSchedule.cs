using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class WorkerSchedule
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public int ScheduleId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Schedule Schedule { get; set; }
        public virtual Worker Worker { get; set; }
    }
}
