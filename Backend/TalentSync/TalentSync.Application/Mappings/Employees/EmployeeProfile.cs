using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Employees;
using TalentSync.Domain.Entities.HumanResources;

namespace TalentSync.Application.Mappings.Employees
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() {
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(
                    d => d.Id,
                    o => o.MapFrom(s => s.UserId))
                .ForMember(
                    d => d.Name,
                    o => o.MapFrom(s => s.User.Name))
                .ForMember(
                    d => d.Email,
                    o => o.MapFrom(s => s.User.Email))
                .ForMember(
                    d => d.Phone,
                    o => o.MapFrom(s => s.User.Phone))
                .ForMember(
                    d => d.ProfilePictureUrl,
                    o => o.MapFrom(s => s.User.ProfilePictureUrl));
        }
    }
}
