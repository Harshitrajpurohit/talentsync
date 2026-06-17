using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Mappings.UserMappings
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleRequestDTO, UserRole>();
            CreateMap<UserRole, UserRoleResponseWithExtraDto>();

        }
    }
}
