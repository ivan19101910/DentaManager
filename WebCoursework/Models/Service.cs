using System;
using System.Collections.Generic;

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
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ServiceTypeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}
