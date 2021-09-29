using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class PatientHospitalizedProfileService : IPatientHospitalizedProfileService
    {
        private readonly IRepository<PatientHospitalizedProfile> _patientHospitalizedProfileRepository;

        public PatientHospitalizedProfileService(
            IRepository<PatientHospitalizedProfile> patientHospitalizedProfileRepository)
        {
            _patientHospitalizedProfileRepository = patientHospitalizedProfileRepository;
        }

        public Task<PatientHospitalizedProfileResponseDto> GetPatientProfilesByPatientId(long patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientHospitalizedProfileResponseDto> CreatePatientProfile(
            CreatePatientHospitalizedProfileDto request)
        {
            var patientHospitalizedProfile = new PatientHospitalizedProfile()
            {
                CreatedAt = DateTime.UtcNow,
                Description = request.Description,
                DiseaseName = request.DiseaseName,
                PatientId = request.PatientId,
                RevisitDate = request.RevisitDate,
            };
            patientHospitalizedProfile =
                await _patientHospitalizedProfileRepository.AddAsync(patientHospitalizedProfile);
            return new PatientHospitalizedProfileResponseDto()
            {
                Description = patientHospitalizedProfile.Description,
                DiseaseName = patientHospitalizedProfile.DiseaseName,
                PatientId = patientHospitalizedProfile.PatientId,
                Id = patientHospitalizedProfile.Id,
                RevisitDate = patientHospitalizedProfile.RevisitDate
            };
        }

        public async Task<PatientHospitalizedProfileResponseDto> EditPatientProfile(long id,
            CreatePatientHospitalizedProfileDto request)
        {
            var patientHospitalizedProfile = await _patientHospitalizedProfileRepository.GetByIdAsync(id);
            if (patientHospitalizedProfile == null)
            {
                throw new ArgumentException($"Cannot find patient hospitalized profile with id: {id}");
            }

            patientHospitalizedProfile.UpdatedAt = DateTime.UtcNow;
            patientHospitalizedProfile.Description = request.Description;
            patientHospitalizedProfile.DiseaseName = request.DiseaseName;
            patientHospitalizedProfile.RevisitDate = request.RevisitDate;
            await _patientHospitalizedProfileRepository.UpdateAsync(patientHospitalizedProfile);
            return new PatientHospitalizedProfileResponseDto()
            {
                Description = patientHospitalizedProfile.Description,
                DiseaseName = patientHospitalizedProfile.DiseaseName,
                PatientId = patientHospitalizedProfile.PatientId,
                Id = patientHospitalizedProfile.Id,
                RevisitDate = patientHospitalizedProfile.RevisitDate
            };
        }

        public async Task DeletePatientProfile(long id)
        {
            var patientHospitalizedProfile = await _patientHospitalizedProfileRepository.GetByIdAsync(id);
            if (patientHospitalizedProfile == null)
            {
                throw new ArgumentException($"Cannot find patient hospitalized profile with id: {id}");
            }

            await _patientHospitalizedProfileRepository.DeleteAsync(patientHospitalizedProfile);
        }

        public async Task<IEnumerable<PatientHospitalizedProfileResponseDto>> GetPatientHospitalizedProfilesForPatient(
            long patientId)
        {
            var @spec = new GetPatientHospitalizedProfilesByPatientId(patientId);
            var patientHospitalizedProfiles = await _patientHospitalizedProfileRepository.ListAsync(@spec);

            return patientHospitalizedProfiles.Select(patientHospitalizedProfile =>
                new PatientHospitalizedProfileResponseDto
                {
                    Description = patientHospitalizedProfile.Description,
                    DiseaseName = patientHospitalizedProfile.DiseaseName,
                    PatientId = patientHospitalizedProfile.PatientId,
                    Id = patientHospitalizedProfile.Id,
                    RevisitDate = patientHospitalizedProfile.RevisitDate
                });
        }
    }
}