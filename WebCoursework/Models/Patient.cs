using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebCoursework
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int PatientId { get; set; }
        [DisplayName("Ім'я")]
        public string FirstName { get; set; }
        [DisplayName("Прізвище")]
        public string LastName { get; set; }
        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Дата народження")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
