using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class Position
    {
        public Position()
        {
            Workers = new HashSet<Worker>();
        }

        public int PositionId { get; set; }
        [DisplayName("Відсоток від прийому")]
        public decimal AppointmentPercentage { get; set; }
        [DisplayName("Базова ставка")]
        public decimal BaseRate { get; set; }
        [DisplayName("Посада")]
        public string PositionName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
