using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace WebCoursework
{
    public partial class SalaryPayment
    {
        public int SalaryPaymentId { get; set; }
        [DisplayName("Місяць")]
        public short MonthNumber { get; set; }
        [DisplayName("Рік")]
        public short Year { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public int TransactionNumber { get; set; }
        [DisplayName("Сума")]
        public decimal Amount { get; set; }
        [DisplayName("Працівник")]
        public int WorkerId { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime CreatedDateTime { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public DateTime LastModifiedDateTime { get; set; }
        [DisplayName("Працівник")]
        public virtual Worker Worker { get; set; }
    }
}
