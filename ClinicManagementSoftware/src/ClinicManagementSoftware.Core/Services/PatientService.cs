﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.Patient;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using static System.Enum;

namespace ClinicManagementSoftware.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public PatientService(IRepository<Patient> patientRepository,
            IMapper mapper, IUserContext userContext)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<PatientDto> GetByIdAsync(long? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var patient = await _patientRepository.GetByIdAsync(id.Value);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Cannot find request with id={id}");
            }

            var patientResult = _mapper.Map<PatientDto>(patient);
            return patientResult;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetPatientsOfClinicSpec(currentUserContext.ClinicId);
            var patients = await _patientRepository.ListAsync(@spec);
            var result = patients.Where(x => x.IsDeleted == (byte) EnumIsDeleted.No)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => _mapper.Map<PatientDto>(x));
            return result;
        }

        public async Task<PatientDto> AddAsync(CreatePatientDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!TryParse(typeof(EnumGender), request.Gender, out var genderResult))
            {
                throw new InvalidGenderException("Have invalid gender when creating request");
            }

            var currentUserContext = await _userContext.GetCurrentContext();
            var patientModel = new Patient
            {
                ClinicId = currentUserContext.ClinicId,
                EmailAddress = request.EmailAddress,
                FullName = request.FullName,
                AddressDetail = request.AddressDetail,
                AddressCity = request.AddressCity,
                AddressDistrict = request.AddressDistrict,
                AddressStreet = request.AddressStreet,
                PhoneNumber = request.PhoneNumber,
                Gender = Convert.ToByte(genderResult),
                DateOfBirth = request.DateOfBirth,
                MedicalInsuranceCode = request.MedicalInsuranceCode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = 0
            };

            var createPatientResult = await _patientRepository.AddAsync(patientModel);
            var patientResult = _mapper.Map<PatientDto>(createPatientResult);
            return patientResult;
        }

        public async Task<PatientDto> UpdateAsync(UpdatePatientDto patientRequest)
        {
            if (patientRequest == null)
            {
                throw new ArgumentNullException(nameof(patientRequest));
            }

            if (!TryParse(typeof(EnumGender), patientRequest.Gender, out var genderResult))
            {
                throw new ArgumentException("Invalid gender");
            }

            // get current user context
            var currentUserContext = await _userContext.GetCurrentContext();
            var patientModel = await _patientRepository.GetByIdAsync(patientRequest.Id);
            if (patientModel == null)
            {
                throw new PatientNotFoundException($"Cannot find request with id: {patientRequest.Id}");
            }

            patientModel.ClinicId = currentUserContext.ClinicId;
            patientModel.EmailAddress = patientRequest.EmailAddress;
            patientModel.FullName = patientRequest.FullName;
            patientModel.PhoneNumber = patientRequest.PhoneNumber;
            patientModel.Gender = Convert.ToByte(genderResult);
            patientModel.UpdatedAt = DateTime.UtcNow;
            patientModel.AddressDetail = patientRequest.AddressDetail;
            patientModel.AddressCity = patientRequest.AddressCity;
            patientModel.AddressDistrict = patientRequest.AddressDistrict;
            patientModel.AddressStreet = patientRequest.AddressStreet;
            patientModel.DateOfBirth = patientRequest.DateOfBirth;
            patientModel.MedicalInsuranceCode = patientRequest.MedicalInsuranceCode;
            patientModel.IsDeleted = 0;
            await _patientRepository.UpdateAsync(patientModel);
            var result = _mapper.Map<PatientDto>(patientModel);
            return result;
        }

        public async Task DeleteAsync(long? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var patient = await _patientRepository.GetByIdAsync(id.Value);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Cannot find a patientRequest with id: {id}");
            }

            patient.IsDeleted = 1;
            patient.DeletedAt = DateTime.UtcNow;
            await _patientRepository.UpdateAsync(patient);
        }
    }
}