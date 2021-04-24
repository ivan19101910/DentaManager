using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("День")]
        public string Name { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
