using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class AppointmentService
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentId { get; set; }
        [DisplayName("Кількість наданої послуги")]
        public int Amount { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("Прийом")]

        public virtual Appointment Appointment { get; set; }
        [DisplayName("Послуга")]
        public virtual Service Service { get; set; }
    }
}
