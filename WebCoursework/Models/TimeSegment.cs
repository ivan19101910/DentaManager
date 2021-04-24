using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
