using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class City
    {
        public City()
        {
            Offices = new HashSet<Office>();
        }

        public int CityId { get; set; }
        [DisplayName("Місто")]
        public string Name { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Office> Offices { get; set; }
    }
}
