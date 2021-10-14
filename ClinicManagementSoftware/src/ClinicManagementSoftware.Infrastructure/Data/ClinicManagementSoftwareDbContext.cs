using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.EFCore.Extensions;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClinicManagementSoftware.Infrastructure.Data
{
    public class ClinicManagementSoftwareDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private static readonly MySqlServerVersion MySqlServerVersion = new(new Version(8, 0, 21));

        public ClinicManagementSoftwareDbContext(DbContextOptions<ClinicManagementSoftwareDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<MedicalServiceGroup> MedicalServiceGroups { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationGroup> MedicationGroups { get; set; }
        public DbSet<PatientHospitalizedProfile> PatientHospitalizedProfiles { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabOrderForm> LabOrderForms { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<VisitingDoctorQueue> VisitingDoctorQueues { get; set; }
        public DbSet<MedicalImageFile> PatientMedicalImageFiles { get; set; }
        public DbSet<CloudinaryFile> CloudinaryFiles { get; set; }
        public DbSet<LabTestQueue> LabTestQueues { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

            // 1 TO MANY
            modelBuilder.Entity<User>()
                .HasOne(user => user.Clinic)
                .WithMany(clinic => clinic.Users);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users);


            modelBuilder.Entity<RolePermission>()
                .HasOne(rolePermission => rolePermission.Role)
                .WithMany(role => role.RolePermissions);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rolePermission => rolePermission.Permission)
                .WithMany(permission => permission.RolePermissions);

            modelBuilder.Entity<Patient>()
                .HasOne(patient => patient.Clinic)
                .WithMany(clinic => clinic.Patients);

            modelBuilder.Entity<MedicalService>()
                .HasOne(medicalService => medicalService.MedicalServiceGroup)
                .WithMany(medicalServiceGroup => medicalServiceGroup.MedicalServices);

            modelBuilder.Entity<MedicalService>()
                .HasOne(medicalService => medicalService.Clinic)
                .WithMany(clinic => clinic.MedicalServices);

            modelBuilder.Entity<Medication>()
                .HasOne(medication => medication.MedicationGroup)
                .WithMany(medicationGroup => medicationGroup.Medications);

            modelBuilder.Entity<PatientHospitalizedProfile>()
                .HasOne(patientHospitalizedProfile => patientHospitalizedProfile.Patient)
                .WithMany(patient => patient.PatientHospitalizedProfiles);

            modelBuilder.Entity<Prescription>()
                .HasOne(prescription => prescription.PatientHospitalizedProfile)
                .WithMany(patientHospitalizedProfile => patientHospitalizedProfile.Prescriptions);

            modelBuilder.Entity<Prescription>()
                .HasOne(prescription => prescription.Doctor)
                .WithMany(user => user.Prescriptions);

            modelBuilder.Entity<LabOrderForm>()
                .HasOne(labOrderForm => labOrderForm.PatientHospitalizedProfile)
                .WithMany(patientHospitalizedProfile => patientHospitalizedProfile.LabOrderForms);


            modelBuilder.Entity<LabOrderForm>()
                .HasOne(labOrderForm => labOrderForm.PatientDoctorVisitForm)
                .WithMany(doctorVisitForm => doctorVisitForm.LabOrderForms);

            modelBuilder.Entity<LabTest>()
                .HasOne(labTest => labTest.LabOrderForm)
                .WithMany(labOrderForm => labOrderForm.LabTests);

            modelBuilder.Entity<LabTest>()
                .HasOne(labTest => labTest.MedicalService)
                .WithMany(medicalService => medicalService.LabTests);

            modelBuilder.Entity<Receipt>()
                .HasOne(receipt => receipt.Patient)
                .WithMany(patient => patient.Receipts);

            modelBuilder.Entity<Receipt>()
                .HasOne(receipt => receipt.PatientDoctorVisitForm)
                .WithOne(doctorVisitForm => doctorVisitForm.Receipt);

            modelBuilder.Entity<VisitingDoctorQueue>()
                .HasOne(visitingDoctorQueue => visitingDoctorQueue.Doctor)
                .WithMany(user => user.VisitingDoctorQueues);

            modelBuilder.Entity<PatientDoctorVisitForm>()
                .HasOne(patientDoctorVisitForm => patientDoctorVisitForm.Patient)
                .WithMany(patient => patient.PatientDoctorVisitForms);

            modelBuilder.Entity<MedicalImageFile>()
                .HasOne(medicalImageFile => medicalImageFile.LabTest)
                .WithMany(hospitalizedProfile => hospitalizedProfile.MedicalImageFiles);

            modelBuilder.Entity<MedicalImageFile>()
                .HasOne(medicalImageFile => medicalImageFile.CloudinaryFile)
                .WithOne(hospitalizedProfile => hospitalizedProfile.MedicalImageFile);

            modelBuilder.Entity<Prescription>()
                .HasOne(prescription => prescription.PatientDoctorVisitForm)
                .WithMany(patientDoctorVisitForm => patientDoctorVisitForm.Prescriptions);

            modelBuilder.Entity<LabTestQueue>()
                .HasOne(labTestQueue => labTestQueue.Clinic)
                .WithMany(clinic => clinic.LabTestQueues);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                _configuration.GetConnectionString(ConfigurationConstant.ClinicManagementSoftwareDatabase);
            optionsBuilder.UseMySql(connectionString, MySqlServerVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}