using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class Service
    {
        public Service()
        {
            AppointmentServices = new HashSet<AppointmentService>();
        }

        public int ServiceId { get; set; }
        [DisplayName("Ціна")]
        public decimal Price { get; set; }
        [DisplayName("Послуга")]
        public string Name { get; set; }
        [DisplayName("Опис")]
        public string Description { get; set; }
        [DisplayName("Вид послуги")]
        public int ServiceTypeId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("Тип послуги")]
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}
