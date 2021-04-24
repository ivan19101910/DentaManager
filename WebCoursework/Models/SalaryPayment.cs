using System;
using System.Collections.Generic;

#nullable disable

namespace WebCoursework
{
    public partial class SalaryPayment
    {
        public int SalaryPaymentId { get; set; }
        public short MonthNumber { get; set; }
        public short Year { get; set; }
        public int TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public int WorkerId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
