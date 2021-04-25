using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebCoursework
{
    public partial class DentalClinicDBContext : DbContext
    {
        public DentalClinicDBContext()
        {
        }

        public DentalClinicDBContext(DbContextOptions<DentalClinicDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentPayment> AppointmentPayments { get; set; }
        public virtual DbSet<AppointmentService> AppointmentServices { get; set; }
        public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<TimeSegment> TimeSegments { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<WorkerSchedule> WorkerSchedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-F1A3NL7;Database=DentalClinicDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_100_CI_AI_KS_WS_SC_UTF8");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("appointmentId");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnType("date")
                    .HasColumnName("appointmentDate");

                entity.Property(e => e.AppointmentTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("appointmentTime");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("notes");

                entity.Property(e => e.PatientId).HasColumnName("patientId");

                entity.Property(e => e.RealEndTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("realEndTime");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.TotalSum)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("totalSum");

                entity.Property(e => e.WorkerId).HasColumnName("workerId");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Appointment_Patient_FK");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Appointment_AppointmentStatus_FK");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Appointment_Worker_FK");
            });

            modelBuilder.Entity<AppointmentPayment>(entity =>
            {
                entity.ToTable("AppointmentPayment");

                entity.Property(e => e.AppointmentPaymentId).HasColumnName("appointmentPaymentId");

                entity.Property(e => e.AppointmentId).HasColumnName("appointmentId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total");

                entity.Property(e => e.TransactionNumber).HasColumnName("transactionNumber");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.AppointmentPayments)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AppointmentPayment_Appointment_FK");
            });

            modelBuilder.Entity<AppointmentService>(entity =>
            {
                entity.ToTable("AppointmentService");

                entity.HasIndex(e => new { e.ServiceId, e.AppointmentId }, "AppointmentService__UN")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.AppointmentId).HasColumnName("appointmentId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ServiceId).HasColumnName("serviceId");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.AppointmentServices)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AppointmentService_Appointment_FK");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.AppointmentServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AppointmentService_Service_FK");
            });

            modelBuilder.Entity<AppointmentStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("AppointmentStatus_PK");

                entity.ToTable("AppointmentStatus");

                entity.HasIndex(e => e.Name, "AppointmentStatus__UN")
                    .IsUnique();

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.HasIndex(e => e.Name, "City__UN")
                    .IsUnique();

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.HasIndex(e => e.Name, "Days__UN")
                    .IsUnique();

                entity.Property(e => e.DayId).HasColumnName("dayId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("Office");

                entity.Property(e => e.OfficeId).HasColumnName("officeId");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Office_City_FK");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.HasIndex(e => e.PhoneNumber, "Patient__UN")
                    .IsUnique();

                entity.Property(e => e.PatientId).HasColumnName("patientId");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position");

                entity.HasIndex(e => e.PositionName, "Position__UN")
                    .IsUnique();

                entity.Property(e => e.PositionId).HasColumnName("positionId");

                entity.Property(e => e.AppointmentPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("appointmentPercentage");

                entity.Property(e => e.BaseRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("baseRate");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("positionName");
            });

            modelBuilder.Entity<SalaryPayment>(entity =>
            {
                entity.ToTable("SalaryPayment");

                entity.Property(e => e.SalaryPaymentId).HasColumnName("salaryPaymentId");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MonthNumber).HasColumnName("monthNumber");

                entity.Property(e => e.TransactionNumber).HasColumnName("transactionNumber");

                entity.Property(e => e.WorkerId).HasColumnName("workerId");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.SalaryPayments)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SalaryPayment_Worker_FK");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DayId).HasColumnName("dayId");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeSegmentId).HasColumnName("timeSegmentId");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.DayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Schedule_Days_FK");

                entity.HasOne(d => d.TimeSegment)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TimeSegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Schedule_TimeSegment_FK");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId).HasColumnName("serviceId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.ServiceTypeId).HasColumnName("serviceTypeId");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Service_ServiceType_FK");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.HasIndex(e => e.Name, "ServiceType__UN")
                    .IsUnique();

                entity.Property(e => e.ServiceTypeId).HasColumnName("serviceTypeId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TimeSegment>(entity =>
            {
                entity.ToTable("TimeSegment");

                entity.Property(e => e.TimeSegmentId).HasColumnName("timeSegmentId");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeEnd)
                    .HasColumnType("time(0)")
                    .HasColumnName("timeEnd");

                entity.Property(e => e.TimeStart)
                    .HasColumnType("time(0)")
                    .HasColumnName("timeStart");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("Worker");

                entity.HasIndex(e => e.PhoneNumber, "Worker__UN")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "Worker__UNv1")
                    .IsUnique();

                entity.Property(e => e.WorkerId).HasColumnName("workerId");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.OfficeId).HasColumnName("officeId");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.PositionId).HasColumnName("positionId");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worker_Office_FK");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worker_Position_FK");
            });

            modelBuilder.Entity<WorkerSchedule>(entity =>
            {
                entity.ToTable("WorkerSchedule");

                entity.HasIndex(e => new { e.WorkerId, e.ScheduleId }, "WorkerSchedule__UN")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastModifiedDateTime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

                entity.Property(e => e.WorkerId).HasColumnName("workerId");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.WorkerSchedules)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerSchedule_Schedule_FK");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.WorkerSchedules)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerSchedule_14_Worker_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
