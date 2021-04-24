using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentPayments = new HashSet<AppointmentPayment>();
            AppointmentServices = new HashSet<AppointmentService>();
        }

        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; }
        public TimeSpan? RealEndTime { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public int WorkerId { get; set; }
        public int PatientId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public decimal? TotalSum { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual AppointmentStatus Status { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual ICollection<AppointmentPayment> AppointmentPayments { get; set; }
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}
