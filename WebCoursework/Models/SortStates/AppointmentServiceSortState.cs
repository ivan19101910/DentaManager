using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCoursework.Models.SortStates
{
    public enum AppointmentServiceSortState
    {
        IdAsc,
        IdDesc,
        DateAsc,
        DateDesc,
        StartTimeAsc,
        StartTimeDesc,
        WorkerNameAsc,
        WorkerNameDesc,
        AmountAsc,
        AmountDesc
    }
}
