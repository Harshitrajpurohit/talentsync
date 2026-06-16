using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Application.Mappings.UserMappings
{
    public class UserProfile : Profile
    {
         public UserProfile() {
            CreateMap<UserRegisterRequestDto, User>()
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.UserRoles, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore());

            CreateMap<User, UserResponseDto>();

            CreateMap<UpdateUserDTO, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.UserRoles, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore())
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));

        }

    }
}
