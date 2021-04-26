using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCoursework.Models.SortStates
{
    public enum AppointmentSortState
    {
        DateAsc,
        DateDesc,
        StartTimeAsc,
        StartTimeDesc,
        EndTimeAsc,
        EndTimeDesc,
        WorkerFirstNameAsc,
        WorkerFirstNameDesc,
        WorkerLastNameAsc,
        WorkerLastNameDesc,
        PatientFirstNameAsc,
        PatientFirstNameDesc,
        PatientLastNameAsc,
        PatientLastNameDesc,
        StatusAsc,
        StatusDesc
    }
}
