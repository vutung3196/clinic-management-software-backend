using System;
using System.Collections.Generic;
using AutoMapper;
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
            //CreateMap<RegistrationUserModel, UserDto>();
        }
    }
}