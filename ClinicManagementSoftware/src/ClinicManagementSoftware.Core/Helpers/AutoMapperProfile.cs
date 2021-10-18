using System;
using System.Collections.Generic;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Dto.Prescription;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserResultResponse>().ForMember(dest => dest.Role,
                    opt
                        => opt.MapFrom(src =>
                            src.Role == null ? null : src.Role.RoleName))
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()));
            CreateMap<UserResultResponse, User>();
            CreateMap<UserDto, User>();
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.Gender,
                    opt
                        => opt.MapFrom(src => src.Gender == null ? string.Empty : ((EnumGender) src.Gender).ToString()))
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()))
                .ForMember(dest => dest.UpdatedAt, opt
                    => opt.MapFrom(src => src.UpdatedAt.Format()));
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.Gender,
                    opt
                        => opt.MapFrom(src => src.Gender == null ? string.Empty : ((EnumGender) src.Gender).ToString()))
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()))
                .ForMember(dest => dest.UpdatedAt, opt
                    => opt.MapFrom(src => src.UpdatedAt.Format()))
                .ForMember(dest => dest.FullName, opt
                    => opt.MapFrom(src => src.FullName));

            CreateMap<Prescription, PrescriptionInformation>()
                .ForMember(dest => dest.RevisitDate,
                    opt => opt.MapFrom(src => src.RevisitDate == null ? string.Empty : src.RevisitDate.Format()))
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()))
                .ForMember(dest => dest.DoctorId, opt
                    => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.DoctorName, opt
                    => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorVisitingFormCode, opt
                    => opt.MapFrom(src => src.PatientDoctorVisitForm.Code))
                .ForMember(dest => dest.MedicationInformation,
                    opt => opt.MapFrom(src => JsonConvert
                        .DeserializeObject<List<MedicationInformation>>(src.MedicationInformation)));

            CreateMap<Receipt, ReceiptResponse>()
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()))
                .ForMember(dest => dest.MedicalServices, opt
                    => opt.MapFrom(src => JsonConvert.DeserializeObject<ICollection<ReceiptMedicalServiceDto>>(src.Services)));

            CreateMap<Prescription, PrescriptionInformation>()
                .ForMember(dest => dest.RevisitDate,
                    opt => opt.MapFrom(src => src.RevisitDate == null ? string.Empty : src.RevisitDate.Format()))
                .ForMember(dest => dest.CreatedAt, opt
                    => opt.MapFrom(src => src.CreatedAt.Format()))
                .ForMember(dest => dest.MedicationInformation,
                    opt => opt.MapFrom(src => JsonConvert
                        .DeserializeObject<List<MedicationInformation>>(src.MedicationInformation)));
        }
    }
}