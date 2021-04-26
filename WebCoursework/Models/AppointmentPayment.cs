using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class AppointmentPayment
    {
        public int AppointmentPaymentId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public int TransactionNumber { get; set; }
        [DisplayName("Номер прийому")]
        public int AppointmentId { get; set; }
        [DisplayName("Сума")]
        public decimal Total { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}
