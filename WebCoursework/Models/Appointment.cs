using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Номер прийому")]
        public int AppointmentId { get; set; }
        [DisplayName("Дата прийому")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        public DateTime AppointmentDate { get; set; }
        [DisplayName("Примітки")]
        public string Notes { get; set; }
        [DisplayName("Час закінчення")]
        [DataType(DataType.Time)]
        public TimeSpan? RealEndTime { get; set; }
        [DisplayName("Час початку")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentTime { get; set; }
        [DisplayName("Лікар")]
        public int WorkerId { get; set; }
        [DisplayName("Пацієнт")]
        public int PatientId { get; set; }
        [DisplayName("Статус")]
        public int StatusId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("Сума за прийом(грн.)")]
        public decimal? TotalSum { get; set; }
        [DisplayName("Пацієнт")]
        public virtual Patient Patient { get; set; }
        [DisplayName("Статус")]
        public virtual AppointmentStatus Status { get; set; }
        [DisplayName("Лікар")]
        public virtual Worker Worker { get; set; }
        public virtual ICollection<AppointmentPayment> AppointmentPayments { get; set; }
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}
