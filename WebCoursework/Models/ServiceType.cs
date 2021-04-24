using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }

        public int ServiceTypeId { get; set; }
        [DisplayName("Тип послуги")]
        public string Name { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
