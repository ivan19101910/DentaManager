using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class AppointmentService
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentId { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Appointment Appointment { get; set; }
        public virtual Service Service { get; set; }
    }
}
