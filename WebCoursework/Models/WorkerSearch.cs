using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebCoursework.Models
{
    public class WorkerSearch
    {
        public int? WorkerId { get; set; }

        [DisplayName("Ім'я")]
        public string FirstName { get; set; }
        [DisplayName("Прізвище")]
        public string LastName { get; set; }
        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Посада")]
        public int? PositionId { get; set; }
        [DisplayName("Офіс")]
        public int? OfficeId { get; set; }

        public virtual Office Office { get; set; }
        public virtual Position Position { get; set; }
    }
}
