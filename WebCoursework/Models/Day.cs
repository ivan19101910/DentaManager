using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class Day
    {
        public Day()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int DayId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
