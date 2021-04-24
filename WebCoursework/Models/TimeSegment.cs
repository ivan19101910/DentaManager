using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Час початку")]
        public TimeSpan TimeStart { get; set; }
        [DisplayName("Час кінця")]
        [DataType(DataType.Time)]
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
