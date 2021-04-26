using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebCoursework.Models;

#nullable disable

namespace WebCoursework
{
    public partial class Worker
    {
        private DentalClinicDBContext Context;
        public Worker()
        {
            Appointments = new HashSet<Appointment>();
            SalaryPayments = new HashSet<SalaryPayment>();
            WorkerSchedules = new HashSet<WorkerSchedule>();
            Context = new DentalClinicDBContext();
        }
        
        public int WorkerId { get; set; }

        [DisplayName("Ім'я")]
        public string FirstName { get; set; }
        [DisplayName("Прізвище")]
        public string LastName { get; set; }
        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Не вказаний Email")]
        public string Email { get; set; }

        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Не вказаний пароль")]
        public string Password { get; set; }
        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Посада")]
        public int PositionId { get; set; }
        [DisplayName("Офіс")]
        public int OfficeId { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }

        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Office Office { get; set; }
        public virtual Position Position { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; }
        public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; }


        public IQueryable<Worker> GetWorkers(WorkerSearch searchModel)
        {
            var result = Context.Workers.AsQueryable();
            
            if (searchModel != null)
            {
                //if (searchModel.Id.HasValue)
                //    result = result.Where(x => x.Id == searchModel.Id);
                //if (!string.IsNullOrEmpty(searchModel.Name))
                //    result = result.Where(x => x.Name.Contains(searchModel.Name));
                //if (searchModel.PriceFrom.HasValue)
                //    result = result.Where(x => x.Price >= searchModel.PriceFrom);
                //if (searchModel.PriceTo.HasValue)
                //    result = result.Where(x => x.Price <= searchModel.PriceTo);
                if(!string.IsNullOrEmpty(searchModel.FirstName))
                    result = result.Where(x => x.FirstName.Contains(searchModel.FirstName));
                if (!string.IsNullOrEmpty(searchModel.LastName))
                    result = result.Where(x => x.LastName.Contains(searchModel.LastName));
            }
            return result;
        }
    }
}
