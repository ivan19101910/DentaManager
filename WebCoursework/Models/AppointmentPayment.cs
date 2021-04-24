using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class AppointmentPayment
    {
        public int AppointmentPaymentId { get; set; }
        public int TransactionNumber { get; set; }
        public int AppointmentId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}
