using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
