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
        [DisplayName("Працівник")]
        public int WorkerId { get; set; }
        [DisplayName("Розклад")]
        public int ScheduleId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("Розклад")]
        public virtual Schedule Schedule { get; set; }
        [DisplayName("Працівник")]
        public virtual Worker Worker { get; set; }
    }
}
