using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("Статус")]
        public string Name { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
