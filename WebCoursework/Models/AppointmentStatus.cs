using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class AppointmentStatus
    {
        public AppointmentStatus()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
