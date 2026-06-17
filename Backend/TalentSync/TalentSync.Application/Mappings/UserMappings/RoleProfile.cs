using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Mappings.UserMappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            CreateMap<Role, RoleResponseDto>();
            CreateMap<CreateRoleDTO, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
