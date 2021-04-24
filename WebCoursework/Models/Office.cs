using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class Office
    {
        public Office()
        {
            Workers = new HashSet<Worker>();
        }

        public int OfficeId { get; set; }
        [DisplayName("Адреса офісу")]
        public string Address { get; set; }
        [DisplayName("Місто")]
        public int CityId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
